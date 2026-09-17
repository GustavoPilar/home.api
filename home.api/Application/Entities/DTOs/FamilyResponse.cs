using home.api.Application.Entities.DTOs.Base;

namespace home.api.Application.Entities.DTOs
{
    /// <summary>
    /// DTO de saída da família. Não expõe proprietário: família é compartilhada.
    /// </summary>
    public class FamilyResponse : ResponseBase
    {
        #region Properties

        /// <summary>
        /// Nome da família
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Identificadores dos membros da família
        /// </summary>
        public ICollection<Guid> Members { get; set; } = new List<Guid>();

        #endregion
    }
}
