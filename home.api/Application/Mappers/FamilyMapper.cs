using home.api.Application.Entities.DTOs;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;

namespace home.api.Application.Mappers
{
    /// <summary>
    /// Mapeamento entre a entidade Family e os seus DTOs
    /// </summary>
    public class FamilyMapper : IFamilyMapper
    {
        #region Members :: ToEntity(), ApplyChanges(), ToResponse(), ToResponseList()

        /// <summary>
        /// Converte o DTO de criação em uma nova família.
        /// Os membros não são mapeados aqui: eles viram associações no serviço.
        /// </summary>
        /// <param name="request">DTO de criação</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        public Family ToEntity(FamilyRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new Family
            {
                Name = request.Name
            };
        }

        /// <summary>
        /// Aplica as alterações do DTO sobre a família rastreada
        /// </summary>
        /// <param name="request">DTO de atualização</param>
        /// <param name="entity">Família rastreada</param>
        /// <exception cref="ArgumentNullException">Requisição ou família nula</exception>
        public void ApplyChanges(FamilyUpdate request, Family entity)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = request.Name;
        }

        /// <summary>
        /// Converte a família no DTO de saída
        /// </summary>
        /// <param name="entity">Família</param>
        /// <exception cref="ArgumentNullException">Família nula</exception>
        public FamilyResponse ToResponse(Family entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            List<Guid> members = new List<Guid>();

            // As associações podem não ter sido carregadas: nesse caso a lista sai vazia, nunca nula
            if (entity.UserFamilies is not null)
            {
                foreach (UserFamily userFamily in entity.UserFamilies)
                {
                    members.Add(userFamily.UserId);
                }
            }

            return new FamilyResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt,
                LastUpdatedAt = entity.LastUpdatedAt,
                Members = members
            };
        }

        /// <summary>
        /// Converte uma coleção reaproveitando o mapeamento unitário (DRY)
        /// </summary>
        /// <param name="entities">Coleção de famílias</param>
        /// <exception cref="ArgumentNullException">Coleção nula</exception>
        public IEnumerable<FamilyResponse> ToResponseList(IEnumerable<Family> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            List<FamilyResponse> responses = new List<FamilyResponse>();

            foreach (Family entity in entities)
            {
                responses.Add(this.ToResponse(entity));
            }

            return responses;
        }

        #endregion
    }
}
