using home.api.Domain.Interfaces.Entities;

namespace home.api.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Acesso a dados comum às entidades que pertencem a um usuário
    /// </summary>
    /// <typeparam name="T">Entidade com proprietário</typeparam>
    public interface IRepositoryBase<T>
        where T : class, IOwnedEntity
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
