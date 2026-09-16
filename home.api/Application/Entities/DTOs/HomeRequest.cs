using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs
{
    public class HomeRequest
    {
        #region Fields

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Length(2, 50, ErrorMessage = "Insira entre 2 a 50 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀÁÂÃÇÉÊÍÓÔÕÚàáâãçéêíóôõú0-9'_’@.#$()\/,\-ºª° ]+$", ErrorMessage = "Insira caracteres válidos.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(8, ErrorMessage = "Insira 8 caracteres.")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Insira apenas números.")]
        public string? ZipCode { get; set; }

        [MaxLength(150, ErrorMessage = "Máximo de 150 caracteres.")]
        [RegularExpression(@"^[A-Za-zÀÁÂÃÇÉÊÍÓÔÕÚàáâãçéêíóôõú0-9'_’@.#$()\/,\-ºª° ]+$", ErrorMessage = "Insira caracteres válidos.")]
        public string? Address { get; set; }

        public int? AddressNumber { get; set; }

        #endregion
    }
}
