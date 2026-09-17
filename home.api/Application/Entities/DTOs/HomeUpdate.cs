using home.api.Application.Entities.DTOs.Base;
using home.api.Utilities;
using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs
{
    /// <summary>
    /// DTO de atualização do lar
    /// </summary>
    public class HomeUpdate : UpdateBase
    {
        #region Properties

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Length(2, 50, ErrorMessage = "Insira entre 2 a 50 caracteres.")]
        [RegularExpression(Global.REGEX_DEFAULT, ErrorMessage = "Insira caracteres válidos.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(8, ErrorMessage = "Insira 8 caracteres.")]
        [RegularExpression(Global.REGEX_ONLY_NUMBERS, ErrorMessage = "Insira apenas números.")]
        public string? ZipCode { get; set; }

        [MaxLength(150, ErrorMessage = "Máximo de 150 caracteres.")]
        [RegularExpression(Global.REGEX_DEFAULT, ErrorMessage = "Insira caracteres válidos.")]
        public string? Address { get; set; }

        public int? AddressNumber { get; set; }

        /// <summary>
        /// Família com quem o lar será compartilhado
        /// </summary>
        public Guid? FamilyId { get; set; }

        #endregion
    }
}
