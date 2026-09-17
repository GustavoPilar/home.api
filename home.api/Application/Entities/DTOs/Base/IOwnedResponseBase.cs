namespace home.api.Application.Entities.DTOs.Base
{
    /// <summary>
    /// DTO de saída de uma entidade que pertence a um usuário
    /// </summary>
    public interface IOwnedResponseBase : IResponseBase
    {
        #region Properties

        Guid UserId { get; set; }

        #endregion
    }
}
