using home.api.Domain.Interfaces.Entities;

namespace home.api.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Contrato de acesso a dados comum a Agregate Root
    /// </summary>
    /// <typeparam name="T">Entidade de domínio</typeparam>
    public interface IRepositoryBase<T>
        where T : class, IEntityBase
    {
        #region Members :: GetEntitiesAsync(), GetByIdAsync(), AddEntity(), UpdateEntity(), DeleteEntity()

        Task<IEnumerable<T>> GetEntitiesAsync(Guid userId);

        Task<T?> GetByIdAsync(Guid userId, Guid entityId);

        void AddEntity(T entity);

        void UpdateEntity(T entity);

        void DeleteEntity(T entity);

        #endregion
    }
}
