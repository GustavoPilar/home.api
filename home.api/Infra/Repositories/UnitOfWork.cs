using home.api.Domain.Interfaces.Entities;
using home.api.Domain.Interfaces.Repositories;
using home.api.Infra.Db;
using System.Collections.Concurrent;

namespace home.api.Infra.Repositories
{
    /// <summary>
    /// Compartilha um único AppDbContext entre os repositórios e centraliza a persistência,
    /// para que várias alterações sejam salvas em uma única transação.
    /// </summary>
    public class UnitOfWork(
        AppDbContext context) : IDisposable
    {
        #region Fields

        private readonly AppDbContext context = context;
        private readonly ConcurrentDictionary<Type, object> repositories = new();
        private bool disposed = false;

        #endregion

        #region Members :: GetRepository(), SaveChangesAsync(), Dispose()

        /// <summary>
        /// Obtém (e reaproveita) o repositório genérico da entidade informada
        /// </summary>
        /// <typeparam name="T">Entidade de domínio</typeparam>
        public IRepositoryBase<T> GetRepository<T>()
            where T : class, IEntityBase
        {
            return (IRepositoryBase<T>)this.repositories.GetOrAdd(
                typeof(T),
                _ => new RepositoryBase<T>(this.context));
        }

        /// <summary>
        /// Persiste todas as alterações pendentes do contexto
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await this.context.SaveChangesAsync();
        }

        /// <summary>
        /// Libera o contexto quando a unidade de trabalho é descartada
        /// </summary>
        /// <param name="disposing">Indica se a liberação foi solicitada explicitamente</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
                this.context.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
