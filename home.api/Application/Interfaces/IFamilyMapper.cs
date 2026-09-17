using home.api.Application.Entities.DTOs.Families;
using home.api.Domain.Entities;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato de conversão da família e do seu catálogo de títulos.
    /// Não usa IMapperBase porque família não tem proprietário e a saída
    /// carrega os membros, que vêm das associações.
    /// </summary>
    public interface IFamilyMapper
    {
        #region Members :: ToEntity(), ApplyChanges(), ToResponse(), ToResponseList(), ToTitleEntity(), ApplyTitleChanges(), ToTitleResponse(), ToTitleResponseList()

        Family ToEntity(FamilyRequest request);

        void ApplyChanges(FamilyUpdate request, Family entity);

        /// <summary>
        /// Converte a família no DTO de saída, incluindo membros, títulos e papéis
        /// </summary>
        /// <param name="entity">Família</param>
        FamilyResponse ToResponse(Family entity);

        IEnumerable<FamilyResponse> ToResponseList(IEnumerable<Family> entities);

        FamilyTitle ToTitleEntity(FamilyTitleRequest request);

        void ApplyTitleChanges(FamilyTitleUpdate request, FamilyTitle entity);

        FamilyTitleResponse ToTitleResponse(FamilyTitle entity);

        IEnumerable<FamilyTitleResponse> ToTitleResponseList(IEnumerable<FamilyTitle> entities);

        #endregion
    }
}
