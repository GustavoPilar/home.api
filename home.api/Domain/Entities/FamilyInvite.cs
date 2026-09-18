using home.api.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    /// <summary>
    /// Convite de entrada em uma família.
    /// Pertence à família, não a um usuário — por isso herda EntityBase.
    /// Com TargetEmail nulo é um link aberto: qualquer pessoa autenticada que
    /// tenha o token entra. Preenchido, só o destinatário consegue aceitar.
    /// </summary>
    public class FamilyInvite : EntityBase
    {
        #region Properties

        /// <summary>
        /// Família de destino
        /// </summary>
        public Guid FamilyId { get; set; }

        /// <summary>
        /// Host que emitiu o convite. É apenas auditoria e não tem chave
        /// estrangeira: uma FK aqui criaria um segundo caminho de cascata
        /// para FamilyInvites, já alcançada pela família.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Hash SHA-256 do token. O token em claro nunca é persistido:
        /// ele é uma credencial ao portador e aparece uma única vez, na criação.
        /// </summary>
        public string TokenHash { get; set; } = string.Empty;

        /// <summary>
        /// E-mail normalizado do destinatário. Nulo indica link aberto.
        /// </summary>
        public string? TargetEmail { get; set; }

        /// <summary>
        /// Momento em que o convite deixa de valer
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Quantas vezes o convite foi aceito. Serve de auditoria, não de limite.
        /// </summary>
        public int UseCount { get; set; }

        /// <summary>
        /// Momento da revogação pelo host
        /// </summary>
        public DateTime? RevokedAt { get; set; }

        #endregion

        #region Navigation

        [JsonIgnore]
        public Family? Family { get; set; }

        #endregion
    }
}
