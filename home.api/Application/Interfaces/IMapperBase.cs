using home.api.Application.Entities.DTOs.Base;
using home.api.Domain.Interfaces.Entities;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato de conversão entre a entidade com proprietário e os seus DTOs.
    /// Mantém o mapeamento fora da responsabilidade do serviço (SRP).
    /// </summary>
    /// <typeparam name="T">Entidade com proprietário</typeparam>
    /// <typeparam name="TRequest">DTO de criação</typeparam>
    /// <typeparam name="TUpdate">DTO de atualização</typeparam>
    /// <typeparam name="TResponse">DTO de saída</typeparam>
    public interface IMapperBase<T, TRequest, TUpdate, TResponse>
        where T : class, IOwnedEntity
        where TRequest : class
        where TUpdate : class, IUpdateBase
        where TResponse : class, IOwnedResponseBase
    {
        #region Members :: ToEntity(), ApplyChanges(), ToResponse(), ToResponseList()

        /// <summary>
        /// Converte o DTO de criação em uma nova entidade, sem preencher identidade e auditoria
        /// </summary>
        /// <param name="request">DTO de criação</param>
        T ToEntity(TRequest request);

        /// <summary>
        /// Aplica as alterações do DTO sobre uma entidade já rastreada pelo contexto
        /// </summary>
        /// <param name="request">DTO de atualização</param>
        /// <param name="entity">Entidade rastreada</param>
        void ApplyChanges(TUpdate request, T entity);

        /// <summary>
        /// Converte a entidade no DTO de saída
        /// </summary>
        /// <param name="entity">Entidade de domínio</param>
        TResponse ToResponse(T entity);

        /// <summary>
        /// Converte uma coleção de entidades nos respectivos DTOs de saída
        /// </summary>
        /// <param name="entities">Coleção de entidades</param>
        IEnumerable<TResponse> ToResponseList(IEnumerable<T> entities);

        #endregion
    }
}
