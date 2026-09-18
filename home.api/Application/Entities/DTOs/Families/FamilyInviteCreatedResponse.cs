namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// Resposta da criação do convite.
    /// É a única vez em que o token aparece em claro — o banco guarda só o hash,
    /// então nem o servidor consegue recuperá-lo depois.
    /// </summary>
    public class FamilyInviteCreatedResponse
    {
        #region Properties

        /// <summary>
        /// Token do convite. Guarde ou compartilhe agora: não é recuperável.
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Dados do convite emitido
        /// </summary>
        public FamilyInviteResponse Invite { get; set; } = new FamilyInviteResponse();

        #endregion
    }
}
