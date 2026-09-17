using home.api.Application.Entities.DTOs.Base;
using home.api.Utilities;
using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs
{
    /// <summary>
    /// DTO de atualização da família
    /// </summary>
    public class FamilyUpdate : UpdateBase
    {
        #region Properties

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Length(2, 50, ErrorMessage = "Quantidade entre 2 e 50 caracteres.")]
        [RegularExpression(Global.REGEX_DEFAULT, ErrorMessage = "Insira caracteres válidos.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Lista completa de membros. Nula mantém os membros atuais;
        /// preenchida, substitui a composição da família.
        /// </summary>
        public ICollection<Guid>? Members { get; set; }

        #endregion
    }
}
