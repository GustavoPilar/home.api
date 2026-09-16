namespace home.api.Domain.Entities
{
    /// <summary>
    /// Lar pertencente a um usuário
    /// </summary>
    public class Home : EntityBase
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

        #endregion
    }
}
