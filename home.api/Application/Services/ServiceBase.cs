using home.api.Application.Entities.DTOs.Base;
using home.api.Application.Interfaces;
using home.api.Domain.Interfaces.Entities;
using home.api.Domain.Interfaces.Repositories;
using home.api.Exceptions;
using home.api.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace home.api.Application.Services
{
    /// <summary>
    /// Serviço genérico de CRUD.
    /// Responde por validação, identidade, auditoria, persistência e log;
    /// a conversão DTO/entidade fica a cargo do mapeador injetado.
    /// </summary>
    /// <typeparam name="T">Entidade de domínio</typeparam>
    /// <typeparam name="TRequest">DTO de criação</typeparam>
    /// <typeparam name="TUpdate">DTO de atualização</typeparam>
    /// <typeparam name="TResponse">DTO de saída</typeparam>
    public class ServiceBase<T, TRequest, TUpdate, TResponse>(
        UnitOfWork unitOfWork,
        IMapperBase<T, TRequest, TUpdate, TResponse> mapper,
        ILogger logger) : IServiceBase<T, TRequest, TUpdate, TResponse>
        where T : class, IEntityBase
        where TRequest : class
        where TUpdate : class, IUpdateBase
        where TResponse : class, IResponseBase
    {
        #region Fields

        protected readonly UnitOfWork unitOfWork = unitOfWork;
        protected readonly IRepositoryBase<T> repositoryBase = unitOfWork.GetRepository<T>();
        protected readonly IMapperBase<T, TRequest, TUpdate, TResponse> mapper = mapper;
        protected readonly ILogger logger = logger;

        #endregion

        #region Members :: GetEntitiesAsync(), GetEntityByIdAsync(), CreateEntityAsync(), UpdateEntityAsync(), DeleteEntityAsync()

        /// <summary>
        /// Lista as entidades do usuário
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <exception cref="ArgumentException">Usuário ID inválido</exception>
        public virtual async Task<IEnumerable<TResponse>> GetEntitiesAsync(Guid userId)
        {
            this.ValidateUserId(userId);

            IEnumerable<T> entities = await this.repositoryBase.GetEntitiesAsync(userId);

            return this.mapper.ToResponseList(entities);
        }

        /// <summary>
        /// Busca uma entidade do usuário pelo identificador
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="entityId">Entidade ID</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        public virtual async Task<TResponse?> GetEntityByIdAsync(Guid userId, Guid entityId)
        {
            this.ValidateUserId(userId);
            this.ValidateUserId(entityId);

            T? entity = await this.repositoryBase.GetByIdAsync(userId, entityId);

            return entity is null ? null : this.mapper.ToResponse(entity);
        }

        /// <summary>
        /// Cria uma entidade vinculada ao usuário autenticado
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de criação</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Usuário ID inválido</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public virtual async Task<TResponse> CreateEntityAsync(Guid userId, TRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateUserId(userId);

            T entity = this.mapper.ToEntity(request);

            // Identidade, propriedade e auditoria são responsabilidade da base: nunca do cliente nem do mapeador
            entity.Id = Guid.CreateVersion7();
            entity.UserId = userId;
            entity.CreatedAt = DateTime.UtcNow;
            entity.LastUpdatedAt = null;

            this.repositoryBase.AddEntity(entity);

            await this.PersistAsync("criar", entity.Id, userId);

            return this.mapper.ToResponse(entity);
        }

        /// <summary>
        /// Atualiza uma entidade do usuário autenticado
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de atualização</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="EntityNotFoundException">Entidade inexistente ou de outro usuário</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public virtual async Task<TResponse> UpdateEntityAsync(Guid userId, TUpdate request)
        {
            ArgumentNullException.ThrowIfNull(request);
            this.ValidateUserId(userId);
            this.ValidateUserId(request.Id);

            // A busca filtrada por usuário é o que impede alterar a entidade de terceiros
            T? entity = await this.repositoryBase.GetByIdAsync(userId, request.Id);

            if (entity is null)
                throw new EntityNotFoundException($"{typeof(T).Name} não encontrado.");

            this.mapper.ApplyChanges(request, entity);

            entity.LastUpdatedAt = DateTime.UtcNow;

            this.repositoryBase.UpdateEntity(entity);

            await this.PersistAsync("atualizar", entity.Id, userId);

            return this.mapper.ToResponse(entity);
        }

        /// <summary>
        /// Remove uma entidade do usuário autenticado
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="entityId">Entidade ID</param>
        /// <exception cref="ArgumentException">Identificador inválido</exception>
        /// <exception cref="PersistenceException">Falha ao persistir</exception>
        public virtual async Task<bool> DeleteEntityAsync(Guid userId, Guid entityId)
        {
            this.ValidateUserId(userId);
            this.ValidateUserId(entityId);

            T? entity = await this.repositoryBase.GetByIdAsync(userId, entityId);

            if (entity is null)
                return false;

            this.repositoryBase.DeleteEntity(entity);

            await this.PersistAsync("excluir", entityId, userId);

            return true;
        }

        #endregion

        #region Helpers :: PersistAsync(), ValidateUserId(), ValidateEntityId()

        /// <summary>
        /// Salva as alterações pendentes e registra o resultado
        /// </summary>
        /// <param name="operation">Nome da operação, usado na mensagem e no log</param>
        /// <param name="entityId">Entidade ID</param>
        /// <param name="userId">Usuário ID</param>
        /// <exception cref="PersistenceException">Falha ou nenhum registro afetado</exception>
        protected async Task PersistAsync(string operation, Guid entityId, Guid userId)
        {
            int affectedRows;

            try
            {
                affectedRows = await this.unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                this.logger.LogError(
                    exception,
                    "Falha ao {Operation} a entidade {EntityType} {EntityId} do usuário {UserId}.",
                    operation, typeof(T).Name, entityId, userId);

                throw new PersistenceException($"Não foi possível {operation} o registro.", exception);
            }

            if (affectedRows == 0)
            {
                this.logger.LogWarning(
                    "Nenhum registro afetado ao {Operation} a entidade {EntityType} {EntityId} do usuário {UserId}.",
                    operation, typeof(T).Name, entityId, userId);

                throw new PersistenceException($"Não foi possível {operation} o registro.");
            }

            this.logger.LogInformation(
                "Operação de {Operation} concluída na entidade {EntityType} {EntityId} do usuário {UserId}.",
                operation, typeof(T).Name, entityId, userId);
        }

        /// <summary>
        /// Valida o identificador
        /// </summary>
        /// <param name="userId">ID</param>
        /// <exception cref="ArgumentException">ID inválido</exception>
        protected void ValidateUserId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id inválido.", nameof(id));
        }

        #endregion
    }
}
