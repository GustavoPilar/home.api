using home.api.Application.Entities.DTOs.Base;
using home.api.Application.Interfaces;
using home.api.Domain.Interfaces.Entities;

namespace home.api.Application.Mappers
{
    /// <summary>
    /// Base dos mapeadores: concentra o que é comum a todas as entidades
    /// e deixa para o mapeador concreto apenas os campos de negócio.
    /// </summary>
    /// <typeparam name="T">Entidade de domínio</typeparam>
    /// <typeparam name="TRequest">DTO de criação</typeparam>
    /// <typeparam name="TUpdate">DTO de atualização</typeparam>
    /// <typeparam name="TResponse">DTO de saída</typeparam>
    public abstract class MapperBase<T, TRequest, TUpdate, TResponse> : IMapperBase<T, TRequest, TUpdate, TResponse>
        where T : class, IEntityBase
        where TRequest : class
        where TUpdate : class, IUpdateBase
        where TResponse : class, IResponseBase
    {
        #region Members :: ToEntity(), ApplyChanges(), ToResponse(), ToResponseList()

        /// <summary>
        /// Converte o DTO de criação em uma nova entidade, sem preencher identidade e auditoria
        /// </summary>
        /// <param name="request">DTO de criação</param>
        public abstract T ToEntity(TRequest request);

        /// <summary>
        /// Aplica as alterações do DTO sobre uma entidade já rastreada pelo contexto
        /// </summary>
        /// <param name="request">DTO de atualização</param>
        /// <param name="entity">Entidade rastreada</param>
        public abstract void ApplyChanges(TUpdate request, T entity);

        /// <summary>
        /// Converte a entidade no DTO de saída
        /// </summary>
        /// <param name="entity">Entidade de domínio</param>
        public abstract TResponse ToResponse(T entity);

        /// <summary>
        /// Converte uma coleção reaproveitando o mapeamento unitário (DRY)
        /// </summary>
        /// <param name="entities">Coleção de entidades</param>
        /// <exception cref="ArgumentNullException">Coleção nula</exception>
        public IEnumerable<TResponse> ToResponseList(IEnumerable<T> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            List<TResponse> responses = new List<TResponse>();

            foreach (T entity in entities)
            {
                responses.Add(this.ToResponse(entity));
            }

            return responses;
        }

        #endregion

        #region Helpers :: FillResponseBase()

        /// <summary>
        /// Copia os campos de identidade e auditoria para o DTO de saída
        /// </summary>
        /// <param name="entity">Entidade de origem</param>
        /// <param name="response">DTO de destino</param>
        protected void FillResponseBase(T entity, TResponse response)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(response);

            response.Id = entity.Id;
            response.UserId = entity.UserId;
            response.CreatedAt = entity.CreatedAt;
            response.LastUpdatedAt = entity.LastUpdatedAt;
        }

        #endregion
    }
}
