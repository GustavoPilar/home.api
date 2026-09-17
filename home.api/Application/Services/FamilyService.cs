using home.api.Application.Entities.DTOs.Families;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;
using home.api.Domain.Enums;
using home.api.Domain.Interfaces.Repositories;
using home.api.Exceptions;
using home.api.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace home.api.Application.Services
{
    /// <summary>
    /// Serviço de famílias. A leitura é decidida pela associação UserFamily,
    /// para que qualquer membro enxergue a família; a escrita é restrita ao host.
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

        #region Members :: Família

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
        /// Cria a família, associa o autor como host e vincula os demais membros
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de criação</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Usuário ID ou título inválido</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<FamilyResponse> CreateEntityAsync(Guid userId, FamilyRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateId(userId, nameof(userId));

            Family family = this.mapper.ToEntity(request);

            family.Id = Guid.CreateVersion7();
            family.CreatedAt = DateTime.UtcNow;
            family.LastUpdatedAt = null;

            Dictionary<Guid, FamilyMemberRequest> desired = this.BuildDesiredMembers(request.Members);

            // O autor é sempre host: sem isso ele perderia o controle do que acabou de criar
            desired[userId] = new FamilyMemberRequest
            {
                UserId = userId,
                FamilyTitleId = desired.TryGetValue(userId, out FamilyMemberRequest? own) ? own.FamilyTitleId : null,
                Role = MembershipRole.Host
            };

            // Família recém-criada ainda não tem títulos próprios: só os globais são válidos
            await this.EnsureTitlesAreAvailableAsync(family.Id, desired.Values);

            this.familyRepository.AddFamily(family);

            List<UserFamily> members = new List<UserFamily>();

            foreach (FamilyMemberRequest member in desired.Values)
            {
                UserFamily association = this.CreateMember(family.Id, member);

                this.familyRepository.AddMember(association);
                members.Add(association);
            }

            family.UserFamilies = members;

            await this.PersistAsync("criar", family.Id, userId);

            Family? created = await this.familyRepository.GetByIdForMemberAsync(userId, family.Id);

            return this.mapper.ToResponse(created ?? family);
        }

        /// <summary>
        /// Atualiza a família e, se informada, a composição de membros
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de atualização</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Identificador ou título inválido</exception>
        /// <exception cref="EntityNotFoundException">Família inexistente ou fora do alcance</exception>
        /// <exception cref="ForbiddenOperationException">Usuário não é host</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<FamilyResponse> UpdateEntityAsync(Guid userId, FamilyUpdate request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(request.Id, nameof(request));

            await this.EnsureHostAsync(userId, request.Id);

            Family? family = await this.familyRepository.GetByIdForMemberAsync(userId, request.Id);

            if (family is null)
                throw new EntityNotFoundException("Família não encontrada.");

            this.mapper.ApplyChanges(request, family);

            family.LastUpdatedAt = DateTime.UtcNow;

            if (request.Members is not null)
                await this.SyncMembersAsync(family, request.Members);

            await this.PersistAsync("atualizar", family.Id, userId);

            Family? updated = await this.familyRepository.GetByIdForMemberAsync(userId, family.Id);

            return this.mapper.ToResponse(updated ?? family);
        }

        /// <summary>
        /// Remove a família; associações e títulos próprios caem por cascata
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="ForbiddenOperationException">Usuário não é host</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<bool> DeleteEntityAsync(Guid userId, Guid familyId)
        {
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));

            UserFamily? membership = await this.familyRepository.GetMembershipAsync(userId, familyId);

            if (membership is null)
                return false;

            if (membership.Role != MembershipRole.Host)
                throw new ForbiddenOperationException("Apenas o host pode excluir a família.");

            Family? family = await this.familyRepository.GetByIdForMemberAsync(userId, familyId);

            if (family is null)
                return false;

            this.familyRepository.DeleteFamily(family);

            await this.PersistAsync("excluir", familyId, userId);

            return true;
        }

        /// <summary>
        /// Remove o próprio usuário da família, transferindo o comando quando necessário
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">Indicação do novo host</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="ForbiddenOperationException">Saída deixaria a família sem host</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<bool> LeaveAsync(Guid userId, Guid familyId, FamilyLeaveRequest? request)
        {
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));

            UserFamily? membership = await this.familyRepository.GetMembershipAsync(userId, familyId);

            if (membership is null)
                return false;

            IEnumerable<UserFamily> members = await this.familyRepository.GetMembersAsync(familyId);

            List<UserFamily> others = members.Where(x => x.UserId != userId).ToList();

            if (membership.Role == MembershipRole.Host && !others.Any(x => x.Role == MembershipRole.Host))
            {
                if (others.Count == 0)
                    throw new ForbiddenOperationException("Você é o único membro da família. Exclua a família em vez de sair dela.");

                Guid? newHostId = request?.NewHostId;

                if (newHostId is null || newHostId == Guid.Empty)
                    throw new ForbiddenOperationException("Indique outro membro como host antes de sair da família.");

                UserFamily? successor = others.FirstOrDefault(x => x.UserId == newHostId);

                if (successor is null)
                    throw new ForbiddenOperationException("O membro indicado como host não pertence a esta família.");

                successor.Role = MembershipRole.Host;
                successor.LastUpdatedAt = DateTime.UtcNow;

                this.logger.LogInformation(
                    "Host da família {FamilyId} transferido do usuário {UserId} para {NewHostId}.",
                    familyId, userId, successor.UserId);
            }

            this.familyRepository.RemoveMember(membership);

            await this.PersistAsync("sair da", familyId, userId);

            return true;
        }

        #endregion

        #region Members :: Títulos

        /// <summary>
        /// Lista os títulos disponíveis para a família
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="EntityNotFoundException">Usuário não é membro</exception>
        public async Task<IEnumerable<FamilyTitleResponse>> GetTitlesAsync(Guid userId, Guid familyId)
        {
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));

            await this.EnsureMembershipAsync(userId, familyId);

            IEnumerable<FamilyTitle> titles = await this.familyRepository.GetTitlesAsync(familyId);

            return this.mapper.ToTitleResponseList(titles);
        }

        /// <summary>
        /// Cria um título próprio da família
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">DTO de criação</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Identificador inválido ou nome já usado</exception>
        /// <exception cref="ForbiddenOperationException">Usuário não é host</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<FamilyTitleResponse> CreateTitleAsync(Guid userId, Guid familyId, FamilyTitleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));

            await this.EnsureHostAsync(userId, familyId);

            // O índice único não cobre os globais no Oracle, por isso a checagem aqui
            if (await this.familyRepository.TitleNameExistsAsync(familyId, request.Name, null))
                throw new ArgumentException("Já existe um título com esse nome disponível para a família.", nameof(request));

            FamilyTitle title = this.mapper.ToTitleEntity(request);

            title.Id = Guid.CreateVersion7();
            title.FamilyId = familyId;
            title.CreatedAt = DateTime.UtcNow;
            title.LastUpdatedAt = null;

            this.familyRepository.AddTitle(title);

            await this.PersistAsync("criar o título da", familyId, userId);

            return this.mapper.ToTitleResponse(title);
        }

        /// <summary>
        /// Renomeia um título próprio da família
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">DTO de atualização</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Identificador inválido ou nome já usado</exception>
        /// <exception cref="EntityNotFoundException">Título inexistente</exception>
        /// <exception cref="ForbiddenOperationException">Usuário não é host, ou título global</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<FamilyTitleResponse> UpdateTitleAsync(Guid userId, Guid familyId, FamilyTitleUpdate request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));
            this.ValidateId(request.Id, nameof(request));

            await this.EnsureHostAsync(userId, familyId);

            FamilyTitle title = await this.EnsureOwnTitleAsync(familyId, request.Id);

            if (await this.familyRepository.TitleNameExistsAsync(familyId, request.Name, title.Id))
                throw new ArgumentException("Já existe um título com esse nome disponível para a família.", nameof(request));

            this.mapper.ApplyTitleChanges(request, title);

            title.LastUpdatedAt = DateTime.UtcNow;

            await this.PersistAsync("atualizar o título da", familyId, userId);

            return this.mapper.ToTitleResponse(title);
        }

        /// <summary>
        /// Remove um título próprio da família; quem o usava fica sem rótulo
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="titleId">Título ID</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="ForbiddenOperationException">Usuário não é host, ou título global</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public async Task<bool> DeleteTitleAsync(Guid userId, Guid familyId, Guid titleId)
        {
            this.ValidateId(userId, nameof(userId));
            this.ValidateId(familyId, nameof(familyId));
            this.ValidateId(titleId, nameof(titleId));

            await this.EnsureHostAsync(userId, familyId);

            FamilyTitle? title = await this.familyRepository.GetTitleByIdAsync(titleId);

            if (title is null || title.FamilyId != familyId)
                return false;

            this.familyRepository.RemoveTitle(title);

            await this.PersistAsync("excluir o título da", familyId, userId);

            return true;
        }

        #endregion

        #region Helpers :: EnsureMembershipAsync(), EnsureHostAsync(), EnsureOwnTitleAsync(), EnsureTitlesAreAvailableAsync(), SyncMembersAsync(), BuildDesiredMembers(), CreateMember(), PersistAsync(), ValidateId()

        /// <summary>
        /// Garante que o usuário pertence à família
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        /// <exception cref="EntityNotFoundException">Usuário não é membro</exception>
        private async Task<UserFamily> EnsureMembershipAsync(Guid userId, Guid familyId)
        {
            UserFamily? membership = await this.familyRepository.GetMembershipAsync(userId, familyId);

            // Quem não é membro recebe "não encontrada", para não revelar a existência da família
            if (membership is null)
                throw new EntityNotFoundException("Família não encontrada.");

            return membership;
        }

        /// <summary>
        /// Garante que o usuário administra a família
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="familyId">Família ID</param>
        /// <exception cref="ForbiddenOperationException">Usuário é membro comum</exception>
        private async Task EnsureHostAsync(Guid userId, Guid familyId)
        {
            UserFamily membership = await this.EnsureMembershipAsync(userId, familyId);

            if (membership.Role != MembershipRole.Host)
                throw new ForbiddenOperationException("Apenas o host da família pode executar esta operação.");
        }

        /// <summary>
        /// Garante que o título existe e pertence à própria família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="titleId">Título ID</param>
        /// <exception cref="EntityNotFoundException">Título inexistente</exception>
        /// <exception cref="ForbiddenOperationException">Título global ou de outra família</exception>
        private async Task<FamilyTitle> EnsureOwnTitleAsync(Guid familyId, Guid titleId)
        {
            FamilyTitle? title = await this.familyRepository.GetTitleByIdAsync(titleId);

            if (title is null)
                throw new EntityNotFoundException("Título não encontrado.");

            if (title.FamilyId is null)
                throw new ForbiddenOperationException("Títulos globais não podem ser alterados nem removidos.");

            if (title.FamilyId != familyId)
                throw new EntityNotFoundException("Título não encontrado.");

            return title;
        }

        /// <summary>
        /// Garante que todos os títulos referenciados são globais ou da própria família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="members">Membros informados</param>
        /// <exception cref="ArgumentException">Título indisponível para a família</exception>
        private async Task EnsureTitlesAreAvailableAsync(Guid familyId, IEnumerable<FamilyMemberRequest> members)
        {
            List<Guid> titleIds = members
                .Where(x => x.FamilyTitleId.HasValue)
                .Select(x => x.FamilyTitleId!.Value)
                .ToList();

            if (!await this.familyRepository.TitlesAreAvailableAsync(familyId, titleIds))
                throw new ArgumentException("Título indisponível para esta família.", nameof(members));
        }

        /// <summary>
        /// Ajusta presença, título e papel para refletir a composição informada
        /// </summary>
        /// <param name="family">Família rastreada</param>
        /// <param name="requested">Composição desejada</param>
        /// <exception cref="ArgumentException">Composição vazia ou título inválido</exception>
        /// <exception cref="ForbiddenOperationException">Composição deixaria a família sem host</exception>
        private async Task SyncMembersAsync(Family family, ICollection<FamilyMemberRequest> requested)
        {
            Dictionary<Guid, FamilyMemberRequest> desired = this.BuildDesiredMembers(requested);

            if (desired.Count == 0)
                throw new ArgumentException("A família precisa de ao menos um membro.", nameof(requested));

            // Validação antes de qualquer alteração, para não deixar o contexto sujo
            if (!desired.Values.Any(x => x.Role == MembershipRole.Host))
                throw new ForbiddenOperationException("A família precisa de ao menos um host.");

            await this.EnsureTitlesAreAvailableAsync(family.Id, desired.Values);

            IEnumerable<UserFamily> current = await this.familyRepository.GetMembersAsync(family.Id);

            HashSet<Guid> currentIds = new HashSet<Guid>();

            foreach (UserFamily member in current)
            {
                currentIds.Add(member.UserId);

                if (desired.TryGetValue(member.UserId, out FamilyMemberRequest? wanted))
                {
                    member.FamilyTitleId = wanted.FamilyTitleId;
                    member.Role = wanted.Role;
                    member.LastUpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    this.familyRepository.RemoveMember(member);
                }
            }

            foreach (KeyValuePair<Guid, FamilyMemberRequest> entry in desired)
            {
                if (!currentIds.Contains(entry.Key))
                    this.familyRepository.AddMember(this.CreateMember(family.Id, entry.Value));
            }
        }

        /// <summary>
        /// Normaliza a lista informada, descartando identificadores vazios e duplicatas
        /// </summary>
        /// <param name="members">Membros informados</param>
        private Dictionary<Guid, FamilyMemberRequest> BuildDesiredMembers(IEnumerable<FamilyMemberRequest>? members)
        {
            Dictionary<Guid, FamilyMemberRequest> desired = new Dictionary<Guid, FamilyMemberRequest>();

            if (members is null)
                return desired;

            foreach (FamilyMemberRequest member in members)
            {
                if (member is not null && member.UserId != Guid.Empty)
                    desired[member.UserId] = member;
            }

            return desired;
        }

        /// <summary>
        /// Cria uma associação entre usuário e família
        /// </summary>
        /// <param name="familyId">Família ID</param>
        /// <param name="member">Dados do membro</param>
        private UserFamily CreateMember(Guid familyId, FamilyMemberRequest member)
        {
            return new UserFamily
            {
                Id = Guid.CreateVersion7(),
                FamilyId = familyId,
                UserId = member.UserId,
                FamilyTitleId = member.FamilyTitleId,
                Role = member.Role,
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
                // Membro inexistente cai aqui: a FK de UserFamilies para AspNetUsers é quem valida
                this.logger.LogError(
                    exception,
                    "Falha ao {Operation} família {FamilyId} pelo usuário {UserId}.",
                    operation, familyId, userId);

                throw new PersistenceException($"Não foi possível {operation} família.", exception);
            }

            if (affectedRows == 0)
            {
                this.logger.LogWarning(
                    "Nenhum registro afetado ao {Operation} família {FamilyId} pelo usuário {UserId}.",
                    operation, familyId, userId);

                throw new PersistenceException($"Não foi possível {operation} família.");
            }

            this.logger.LogInformation(
                "Operação de {Operation} família {FamilyId} concluída pelo usuário {UserId}.",
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
