using home.api.Utilities;
using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// DTO de criação de um título próprio da família
    /// </summary>
    public class FamilyTitleRequest
    {
        #region Properties

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Length(2, 50, ErrorMessage = "Quantidade entre 2 e 50 caracteres.")]
        [RegularExpression(Global.REGEX_DEFAULT, ErrorMessage = "Insira caracteres válidos.")]
        public string Name { get; set; } = string.Empty;

        #endregion
    }
}
