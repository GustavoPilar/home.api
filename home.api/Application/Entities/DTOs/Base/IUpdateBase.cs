namespace home.api.Application.Entities.DTOs.Base
{
    /// <summary>
    /// Campo comum a todo DTO de atualização.
    /// O usuário proprietário não trafega no corpo da requisição: ele vem do token.
    /// </summary>
    public interface IUpdateBase
    {
        #region Properties

        Guid Id { get; set; }

        #endregion
    }
}
