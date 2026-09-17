using home.api.Domain.Interfaces.Entities;
using home.api.Domain.Interfaces.Repositories;
using home.api.Infra.Db;
using Microsoft.EntityFrameworkCore;

namespace home.api.Infra.Repositories
{
    /// <summary>
    /// Implementação genérica de acesso a dados das entidades com proprietário
    /// </summary>
    /// <typeparam name="T">Entidade com proprietário</typeparam>
    public class RepositoryBase<T>(
        AppDbContext context) : IRepositoryBase<T>
        where T : class, IOwnedEntity
    {
        #region Fields

        protected readonly AppDbContext context = context;
        protected readonly DbSet<T> dbSet = context.Set<T>();

        #endregion

        #region Members :: GetByIdAsync(), GetEntitiesAsync(), AddEntity(), UpdateEntity(), DeleteEntity()

        /// <summary>
        /// Busca uma entidade do usuário pelo identificador
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        /// <param name="entityId">Entidade ID</param>
        public async Task<T?> GetByIdAsync(Guid userId, Guid entityId)
        {
            return await this.dbSet
                .FirstOrDefaultAsync(x => x.Id == entityId && x.UserId == userId);
        }

        /// <summary>
        /// Lista as entidades do usuário
        /// </summary>
        /// <param name="userId">Usuário ID</param>
        public async Task<IEnumerable<T>> GetEntitiesAsync(Guid userId)
        {
            // AsNoTracking porque a listagem é somente leitura e não precisa do change tracker
            return await this.dbSet
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Marca uma entidade nova para inclusão
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void AddEntity(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            this.dbSet.Add(entity);
        }

        /// <summary>
        /// Marca uma entidade desanexada como alterada
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void UpdateEntity(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            // Entidade já rastreada não pode ser anexada de novo: o change tracker lançaria exceção
            if (this.context.Entry(entity).State == EntityState.Detached)
            {
                this.dbSet.Attach(entity);
                this.context.Entry(entity).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Marca uma entidade para remoção
        /// </summary>
        /// <param name="entity">Entidade</param>
        public void DeleteEntity(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            if (this.context.Entry(entity).State == EntityState.Detached)
                this.dbSet.Attach(entity);

            this.dbSet.Remove(entity);
        }

        #endregion
    }
}
