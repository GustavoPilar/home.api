using home.api.Application.Entities.DTOs;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato do serviço de famílias.
    /// Não herda IServiceBase porque família não tem proprietário: o acesso
    /// é decidido pela associação do usuário, não por um campo UserId.
    /// </summary>
    public interface IFamilyService
    {
        #region Members :: GetEntitiesAsync(), GetEntityByIdAsync(), CreateEntityAsync(), UpdateEntityAsync(), DeleteEntityAsync()

        /// <summary>
        /// Lista as famílias das quais o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        Task<IEnumerable<FamilyResponse>> GetEntitiesAsync(Guid userId);

        /// <summary>
        /// Busca uma família da qual o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        Task<FamilyResponse?> GetEntityByIdAsync(Guid userId, Guid familyId);

        /// <summary>
        /// Cria uma família e associa o criador como membro
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de criação</param>
        Task<FamilyResponse> CreateEntityAsync(Guid userId, FamilyRequest request);

        /// <summary>
        /// Atualiza uma família da qual o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de atualização</param>
        Task<FamilyResponse> UpdateEntityAsync(Guid userId, FamilyUpdate request);

        /// <summary>
        /// Remove uma família da qual o usuário é membro
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        Task<bool> DeleteEntityAsync(Guid userId, Guid familyId);

        #endregion
    }
}
