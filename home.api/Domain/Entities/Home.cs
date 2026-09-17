using home.api.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    /// <summary>
    /// Lar pertencente a um usuário, opcionalmente compartilhado com uma família
    /// </summary>
    public class Home : OwnedEntityBase
    {
        #region Properties

        /// <summary>
        /// Nome do lar
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// CEP do lar
        /// </summary>
        public string? ZipCode { get; set; }

        /// <summary>
        /// Logradouro do lar
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Número do logradouro
        /// </summary>
        public int? AddressNumber { get; set; }

        /// <summary>
        /// Família com quem o lar é compartilhado
        /// </summary>
        public Guid? FamilyId { get; set; }

        #endregion

        #region Navigation

        [JsonIgnore]
        public Family? Family { get; set; }

        #endregion
    }
}
