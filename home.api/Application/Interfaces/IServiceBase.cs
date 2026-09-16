using home.api.Application.Entities.DTOs.Base;
using home.api.Domain.Interfaces.Entities;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato do serviço genérico de CRUD.
    /// Só trafega DTO: a entidade de domínio não sai da camada de aplicação.
    /// </summary>
    /// <typeparam name="T">Entidade de domínio</typeparam>
    /// <typeparam name="TRequest">DTO de criação</typeparam>
    /// <typeparam name="TUpdate">DTO de atualização</typeparam>
    /// <typeparam name="TResponse">DTO de saída</typeparam>
    public interface IServiceBase<T, TRequest, TUpdate, TResponse>
        where T : class, IEntityBase
        where TRequest : class
        where TUpdate : class, IUpdateBase
        where TResponse : class, IResponseBase
    {
        #region Members :: GetEntitiesAsync(), GetEntityByIdAsync(), CreateEntityAsync(), UpdateEntityAsync(), DeleteEntityAsync()

        Task<IEnumerable<TResponse>> GetEntitiesAsync(Guid userId);

        Task<TResponse?> GetEntityByIdAsync(Guid userId, Guid entityId);

        Task<TResponse> CreateEntityAsync(Guid userId, TRequest request);

        Task<TResponse> UpdateEntityAsync(Guid userId, TUpdate request);

        Task<bool> DeleteEntityAsync(Guid userId, Guid entityId);

        #endregion
    }
}
