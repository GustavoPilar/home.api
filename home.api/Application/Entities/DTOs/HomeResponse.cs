using home.api.Application.Entities.DTOs.Base;

namespace home.api.Application.Entities.DTOs
{
    /// <summary>
    /// DTO de saída do lar
    /// </summary>
    public class HomeResponse : OwnedResponseBase
    {
        #region Properties

        public string Name { get; set; } = string.Empty;

        public string? ZipCode { get; set; }

        public string? Address { get; set; }

        public int? AddressNumber { get; set; }

        /// <summary>
        /// Família com quem o lar é compartilhado
        /// </summary>
        public Guid? FamilyId { get; set; }

        #endregion
    }
}
