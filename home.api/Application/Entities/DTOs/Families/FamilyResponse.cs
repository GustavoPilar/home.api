using home.api.Application.Entities.DTOs.Base;

namespace home.api.Application.Entities.DTOs.Families
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
        /// Membros da família, com título e papel
        /// </summary>
        public ICollection<FamilyMemberResponse> Members { get; set; } = new List<FamilyMemberResponse>();

        #endregion
    }
}
