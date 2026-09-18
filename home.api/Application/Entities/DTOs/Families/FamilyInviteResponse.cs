using home.api.Application.Entities.DTOs.Base;

namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// Convite na listagem do host. Nunca carrega o token:
    /// ele só existe em claro na resposta da criação.
    /// </summary>
    public class FamilyInviteResponse : ResponseBase
    {
        #region Properties

        public Guid FamilyId { get; set; }

        /// <summary>
        /// Destinatário do convite dirigido; nulo quando é link aberto
        /// </summary>
        public string? TargetEmail { get; set; }

        /// <summary>
        /// Indica link aberto, sem destinatário definido
        /// </summary>
        public bool IsOpen { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int UseCount { get; set; }

        public DateTime? RevokedAt { get; set; }

        /// <summary>
        /// Convite ainda utilizável: não revogado e dentro da validade
        /// </summary>
        public bool IsActive { get; set; }

        #endregion
    }
}
