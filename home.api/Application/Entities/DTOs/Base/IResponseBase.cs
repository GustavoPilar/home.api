namespace home.api.Application.Entities.DTOs.Base
{
    /// <summary>
    /// Identidade e auditoria comuns a todo DTO de saída
    /// </summary>
    public interface IResponseBase
    {
        #region Properties

        Guid Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
