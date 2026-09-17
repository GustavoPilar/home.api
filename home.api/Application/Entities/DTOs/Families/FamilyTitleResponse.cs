using home.api.Application.Entities.DTOs.Base;

namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// DTO de saída do título
    /// </summary>
    public class FamilyTitleResponse : ResponseBase
    {
        #region Properties

        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Família dona do título; nulo quando é global
        /// </summary>
        public Guid? FamilyId { get; set; }

        /// <summary>
        /// Título global não pode ser alterado nem removido pelo host
        /// </summary>
        public bool IsGlobal { get; set; }

        #endregion
    }
}
