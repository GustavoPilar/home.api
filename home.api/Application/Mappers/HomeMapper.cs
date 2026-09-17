using home.api.Application.Entities.DTOs;
using home.api.Domain.Entities;

namespace home.api.Application.Mappers
{
    /// <summary>
    /// Mapeamento entre a entidade Home e os seus DTOs
    /// </summary>
    public class HomeMapper : MapperBase<Home, HomeRequest, HomeUpdate, HomeResponse>
    {
        #region Members :: ToEntity(), ApplyChanges(), ToResponse()

        /// <summary>
        /// Converte o DTO de criação em uma nova entidade Home
        /// </summary>
        /// <param name="request">DTO de criação</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        public override Home ToEntity(HomeRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new Home
            {
                Name = request.Name,
                ZipCode = request.ZipCode,
                Address = request.Address,
                AddressNumber = request.AddressNumber,
                FamilyId = request.FamilyId
            };
        }

        /// <summary>
        /// Aplica as alterações do DTO sobre a entidade rastreada
        /// </summary>
        /// <param name="request">DTO de atualização</param>
        /// <param name="entity">Entidade rastreada</param>
        /// <exception cref="ArgumentNullException">Requisição ou entidade nula</exception>
        public override void ApplyChanges(HomeUpdate request, Home entity)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = request.Name;
            entity.ZipCode = request.ZipCode;
            entity.Address = request.Address;
            entity.AddressNumber = request.AddressNumber;
            entity.FamilyId = request.FamilyId;
        }

        /// <summary>
        /// Converte a entidade Home no seu DTO de saída
        /// </summary>
        /// <param name="entity">Entidade de domínio</param>
        /// <exception cref="ArgumentNullException">Entidade nula</exception>
        public override HomeResponse ToResponse(Home entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            HomeResponse response = new HomeResponse
            {
                Name = entity.Name,
                ZipCode = entity.ZipCode,
                Address = entity.Address,
                AddressNumber = entity.AddressNumber,
                FamilyId = entity.FamilyId
            };

            this.FillResponseBase(entity, response);

            return response;
        }

        #endregion
    }
}
