namespace home.api.Application.Entities.DTOs.Base
{
    /// <summary>
    /// DTO de saída de uma entidade com proprietário
    /// </summary>
    public class OwnedResponseBase : ResponseBase, IOwnedResponseBase
    {
        #region Properties

        /// <summary>
        /// Usuário proprietário da entidade
        /// </summary>
        public Guid UserId { get; set; }

        #endregion
    }
}
