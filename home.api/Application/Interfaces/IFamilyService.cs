using home.api.Application.Entities.DTOs.Families;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato do serviço de famílias.
    /// Não herda IServiceBase porque família não tem proprietário: o acesso
    /// vem da associação e a permissão de escrita, do papel Host.
    /// </summary>
    public interface IFamilyService
    {
        #region Members :: Família

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
        /// Cria uma família e associa o autor como host
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de criação</param>
        Task<FamilyResponse> CreateEntityAsync(Guid userId, FamilyRequest request);

        /// <summary>
        /// Atualiza a família e, se informada, a composição de membros. Restrito ao host.
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="request">DTO de atualização</param>
        Task<FamilyResponse> UpdateEntityAsync(Guid userId, FamilyUpdate request);

        /// <summary>
        /// Remove a família. Restrito ao host.
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        Task<bool> DeleteEntityAsync(Guid userId, Guid familyId);

        /// <summary>
        /// Remove o próprio usuário da família. Sendo ele o único host,
        /// é obrigatório indicar o sucessor.
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">Indicação do novo host, quando necessária</param>
        Task<bool> LeaveAsync(Guid userId, Guid familyId, FamilyLeaveRequest? request);

        #endregion

        #region Members :: Títulos

        /// <summary>
        /// Lista os títulos disponíveis para a família: globais e próprios
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        Task<IEnumerable<FamilyTitleResponse>> GetTitlesAsync(Guid userId, Guid familyId);

        /// <summary>
        /// Cria um título próprio da família. Restrito ao host.
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">DTO de criação</param>
        Task<FamilyTitleResponse> CreateTitleAsync(Guid userId, Guid familyId, FamilyTitleRequest request);

        /// <summary>
        /// Renomeia um título próprio da família. Restrito ao host.
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="request">DTO de atualização</param>
        Task<FamilyTitleResponse> UpdateTitleAsync(Guid userId, Guid familyId, FamilyTitleUpdate request);

        /// <summary>
        /// Remove um título próprio da família. Restrito ao host.
        /// </summary>
        /// <param name="userId">Usuário ID, obtido do token</param>
        /// <param name="familyId">Família ID</param>
        /// <param name="titleId">Título ID</param>
        Task<bool> DeleteTitleAsync(Guid userId, Guid familyId, Guid titleId);

        #endregion
    }
}
