using home.api.Application.Entities.DTOs.Families;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;

namespace home.api.Application.Mappers
{
    /// <summary>
    /// Mapeamento entre as entidades de família e os seus DTOs
    /// </summary>
    public class FamilyMapper : IFamilyMapper
    {
        #region Members :: Família

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

            List<FamilyMemberResponse> members = new List<FamilyMemberResponse>();

            // As associações podem não ter sido carregadas: nesse caso a lista sai vazia, nunca nula
            if (entity.UserFamilies is not null)
            {
                foreach (UserFamily userFamily in entity.UserFamilies)
                {
                    members.Add(new FamilyMemberResponse
                    {
                        UserId = userFamily.UserId,
                        FamilyTitleId = userFamily.FamilyTitleId,
                        FamilyTitleName = userFamily.FamilyTitle?.Name,
                        Role = userFamily.Role
                    });
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

        #region Members :: Títulos

        /// <summary>
        /// Converte o DTO de criação em um novo título, sem identidade nem família
        /// </summary>
        /// <param name="request">DTO de criação</param>
        /// <exception cref="ArgumentNullException">Requisição nula</exception>
        public FamilyTitle ToTitleEntity(FamilyTitleRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return new FamilyTitle
            {
                Name = request.Name
            };
        }

        /// <summary>
        /// Aplica as alterações do DTO sobre o título rastreado
        /// </summary>
        /// <param name="request">DTO de atualização</param>
        /// <param name="entity">Título rastreado</param>
        /// <exception cref="ArgumentNullException">Requisição ou título nulo</exception>
        public void ApplyTitleChanges(FamilyTitleUpdate request, FamilyTitle entity)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = request.Name;
        }

        /// <summary>
        /// Converte o título no DTO de saída
        /// </summary>
        /// <param name="entity">Título</param>
        /// <exception cref="ArgumentNullException">Título nulo</exception>
        public FamilyTitleResponse ToTitleResponse(FamilyTitle entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new FamilyTitleResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                FamilyId = entity.FamilyId,
                IsGlobal = entity.FamilyId is null,
                CreatedAt = entity.CreatedAt,
                LastUpdatedAt = entity.LastUpdatedAt
            };
        }

        /// <summary>
        /// Converte uma coleção reaproveitando o mapeamento unitário (DRY)
        /// </summary>
        /// <param name="entities">Coleção de títulos</param>
        /// <exception cref="ArgumentNullException">Coleção nula</exception>
        public IEnumerable<FamilyTitleResponse> ToTitleResponseList(IEnumerable<FamilyTitle> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            List<FamilyTitleResponse> responses = new List<FamilyTitleResponse>();

            foreach (FamilyTitle entity in entities)
            {
                responses.Add(this.ToTitleResponse(entity));
            }

            return responses;
        }

        #endregion

        #region Members :: Convites

        /// <summary>
        /// Converte o convite no DTO de saída. O TokenHash nunca sai daqui.
        /// </summary>
        /// <param name="entity">Convite</param>
        /// <exception cref="ArgumentNullException">Convite nulo</exception>
        public FamilyInviteResponse ToInviteResponse(FamilyInvite entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new FamilyInviteResponse
            {
                Id = entity.Id,
                FamilyId = entity.FamilyId,
                TargetEmail = entity.TargetEmail,
                IsOpen = entity.TargetEmail is null,
                ExpiresAt = entity.ExpiresAt,
                UseCount = entity.UseCount,
                RevokedAt = entity.RevokedAt,
                IsActive = entity.RevokedAt is null && entity.ExpiresAt > DateTime.UtcNow,
                CreatedAt = entity.CreatedAt,
                LastUpdatedAt = entity.LastUpdatedAt
            };
        }

        /// <summary>
        /// Converte uma coleção reaproveitando o mapeamento unitário (DRY)
        /// </summary>
        /// <param name="entities">Coleção de convites</param>
        /// <exception cref="ArgumentNullException">Coleção nula</exception>
        public IEnumerable<FamilyInviteResponse> ToInviteResponseList(IEnumerable<FamilyInvite> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            List<FamilyInviteResponse> responses = new List<FamilyInviteResponse>();

            foreach (FamilyInvite entity in entities)
            {
                responses.Add(this.ToInviteResponse(entity));
            }

            return responses;
        }

        #endregion
    }
}
