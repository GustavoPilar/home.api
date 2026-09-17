using home.api.Application.Entities.DTOs;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;
using home.api.Domain.Interfaces.Repositories;
using home.api.Exceptions;
using home.api.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace home.api.Application.Services
{
    /// <summary>
    /// Serviço de famílias. Todo acesso é decidido pela associação UserFamily,
    /// para que qualquer membro — e não apenas quem criou — enxergue a família.
    /// </summary>
    public class FamilyService(
        UnitOfWork unitOfWork,
        IFamilyMapper mapper,
        ILogger<FamilyService> logger) : IFamilyService
    {
        #region Fields

        private readonly UnitOfWork unitOfWork = unitOfWork;
        private readonly IFamilyRepository familyRepository = unitOfWork.FamilyRepository;
        private readonly IFamilyMapper mapper = mapper;
        private readonly ILogger<FamilyService> logger = logger;

        #endregion

        #region Members :: GetEntitiesAsync(), GetEntityByIdAsync(), CreateEntityAsync(), UpdateEntityAsync(), DeleteEntityAsync()

        /// <summary>
        /// Lista as famílias das quais o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <exception cref="ArgumentException">Usuário ID inválido</exception>
        public async Task<IEnumerable<FamilyResponse>> GetEntitiesAsync(Guid userId)
        {
            this.ValidateId(userId, nameof(userId));

            IEnumerable<Family> families = await this.familyRepository.GetFamiliesByMemberAsync(userId);

            return this.mapper.ToResponseList(families);
        }

        /// <summary>
        /// Busca uma família da qual o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        public async Task<FamilyResponse?> GetEntityByIdAsync(Guid userId, Guid familyId)
        {
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));

            Family? family = await this.familyRepository.GetByIdForMemberAsync(userId, familyId);

            return family is null ? null : this.mapper.ToResponse(family);
        }

        /// <summary>
        /// Cria a família e associa o criador, além dos membros informados
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de criação</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Usuário ID inválido</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<FamilyResponse> CreateEntityAsync(Guid userId, FamilyRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateId(userId, nameof(userId));

            Family family = this.mapper.ToEntity(request);

            family.Id = Guid.CreateVersion7();
            family.CreatedAt = DateTime.UtcNow;
            family.LastUpdatedAt = null;

            this.familyRepository.AddFamily(family);

            // O criador entra sempre, senão ele mesmo perderia acesso ao que acabou de criar
            List<UserFamily> members = new List<UserFamily>();

            foreach (Guid memberId in this.BuildMemberIds(userId, request.Members))
            {
                UserFamily member = this.CreateMember(family.Id, memberId);

                this.familyRepository.AddMember(member);
                members.Add(member);
            }

            family.UserFamilies = members;

            await this.PersistAsync("criar", family.Id, userId);

            return this.mapper.ToResponse(family);
        }

        /// <summary>
        /// Atualiza a família e, se informada, a composição de membros
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de atualização</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="EntityNotFoundException">Família inexistente ou de outro grupo</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<FamilyResponse> UpdateEntityAsync(Guid userId, FamilyUpdate request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(request.Id, nameof(request));

            Family? family = await this.familyRepository.GetByIdForMemberAsync(userId, request.Id);

            if (family is null)
                throw new EntityNotFoundException("Família não encontrada.");

            this.mapper.ApplyChanges(request, family);

            family.LastUpdatedAt = DateTime.UtcNow;

            if (request.Members is not null)
                await this.SyncMembersAsync(family, userId, request.Members);

            await this.PersistAsync("atualizar", family.Id, userId);

            Family? updated = await this.familyRepository.GetByIdForMemberAsync(userId, family.Id);

            return this.mapper.ToResponse(updated ?? family);
        }

        /// <summary>
        /// Remove a família; as associações caem por cascata
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<bool> DeleteEntityAsync(Guid userId, Guid familyId)
        {
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));

            Family? family = await this.familyRepository.GetByIdForMemberAsync(userId, familyId);

            if (family is null)
                return false;

            this.familyRepository.DeleteFamily(family);

            await this.PersistAsync("excluir", familyId, userId);

            return true;
        }

        #endregion

        #region Helpers :: SyncMembersAsync(), BuildMemberIds(), CreateMember(), PersistAsync(), ValidateId()

        /// <summary>
        /// Ajusta as associações para refletir a lista informada
        /// </summary>
        /// <param name="family">Família rastreada</param>
        /// <param name="userId">Usuário que está alterando</param>
        /// <param name="memberIds">Composição desejada</param>
        private async Task SyncMembersAsync(Family family, Guid userId, ICollection<Guid> memberIds)
        {
            HashSet<Guid> desired = this.BuildMemberIds(userId, memberIds);

            IEnumerable<UserFamily> current = await this.familyRepository.GetMembersAsync(family.Id);

            HashSet<Guid> currentIds = new HashSet<Guid>();

            foreach (UserFamily member in current)
            {
                currentIds.Add(member.UserId);

                if (!desired.Contains(member.UserId))
                    this.familyRepository.RemoveMember(member);
            }

            foreach (Guid memberId in desired)
            {
                if (!currentIds.Contains(memberId))
                    this.familyRepository.AddMember(this.CreateMember(family.Id, memberId));
            }
        }

        /// <summary>
        /// Monta a composição final garantindo o usuário atual e sem duplicatas
        /// </summary>
        /// <param name="userId">Usuário atual</param>
        /// <param name="memberIds">Membros informados</param>
        private HashSet<Guid> BuildMemberIds(Guid userId, ICollection<Guid>? memberIds)
        {
            HashSet<Guid> ids = new HashSet<Guid> { userId };

            if (memberIds is null)
                return ids;

            foreach (Guid memberId in memberIds)
            {
                if (memberId != Guid.Empty)
                    ids.Add(memberId);
            }

            return ids;
        }

        /// <summary>
        /// Cria uma associação entre usuário e família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="memberId">Usuário ID do membro</param>
        private UserFamily CreateMember(Guid familyId, Guid memberId)
        {
            return new UserFamily
            {
                Id = Guid.CreateVersion7(),
                FamilyId = familyId,
                UserId = memberId,
                CreatedAt = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Salva as alterações pendentes e registra o resultado
        /// </summary>
        /// <param name="operation">Nome da operação</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="userId">Usuário ID</param>
        /// <exception cref="PersistenceException">Falha ou nenhum registro afetado</exception>
        private async Task PersistAsync(string operation, Guid familyId, Guid userId)
        {
            int affectedRows;

            try
            {
                affectedRows = await this.unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                // Membro inexistente cai aqui: a FK de UserFamily para AspNetUsers é quem valida
                this.logger.LogError(
                    exception,
                    "Falha ao {Operation} a família {FamilyId} pelo usuário {UserId}.",
                    operation, familyId, userId);

                throw new PersistenceException($"Não foi possível {operation} a família.", exception);
            }

            if (affectedRows == 0)
            {
                this.logger.LogWarning(
                    "Nenhum registro afetado ao {Operation} a família {FamilyId} pelo usuário {UserId}.",
                    operation, familyId, userId);

                throw new PersistenceException($"Não foi possível {operation} a família.");
            }

            this.logger.LogInformation(
                "Operação de {Operation} concluída na família {FamilyId} pelo usuário {UserId}.",
                operation, familyId, userId);
        }

        /// <summary>
        /// Valida um identificador obrigatório
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <param name="parameterName">Nome do parâmetro, para a mensagem</param>
        /// <exception cref="ArgumentException">Identificador vazio</exception>
        private void ValidateId(Guid id, string parameterName)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Identificador inválido.", parameterName);
        }

        #endregion
    }
}
