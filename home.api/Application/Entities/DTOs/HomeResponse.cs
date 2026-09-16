using home.api.Application.Entities.DTOs.Base;

namespace home.api.Application.Entities.DTOs
{
    /// <summary>
    /// DTO de saída do lar
    /// </summary>
    public class HomeResponse : ResponseBase
    {
        #region Properties

        public string Name { get; set; } = string.Empty;

        public string? ZipCode { get; set; }

        public string? Address { get; set; }

        public int? AddressNumber { get; set; }

        #endregion
    }
}
