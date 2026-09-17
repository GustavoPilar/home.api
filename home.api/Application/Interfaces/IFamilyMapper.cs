using home.api.Application.Entities.DTOs;
using home.api.Domain.Entities;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato de conversão da família.
    /// Não usa IMapperBase porque família não tem proprietário e a saída
    /// carrega a lista de membros, que vem das associações.
    /// </summary>
    public interface IFamilyMapper
    {
        #region Members :: ToEntity(), ApplyChanges(), ToResponse(), ToResponseList()

        /// <summary>
        /// Converte o DTO de criação em uma nova família, sem identidade nem auditoria
        /// </summary>
        /// <param name="request">DTO de criação</param>
        Family ToEntity(FamilyRequest request);

        /// <summary>
        /// Aplica as alterações do DTO sobre a família rastreada, exceto os membros
        /// </summary>
        /// <param name="request">DTO de atualização</param>
        /// <param name="entity">Família rastreada</param>
        void ApplyChanges(FamilyUpdate request, Family entity);

        /// <summary>
        /// Converte a família no DTO de saída, incluindo os membros associados
        /// </summary>
        /// <param name="entity">Família</param>
        FamilyResponse ToResponse(Family entity);

        /// <summary>
        /// Converte uma coleção de famílias nos respectivos DTOs de saída
        /// </summary>
        /// <param name="entities">Coleção de famílias</param>
        IEnumerable<FamilyResponse> ToResponseList(IEnumerable<Family> entities);

        #endregion
    }
}
