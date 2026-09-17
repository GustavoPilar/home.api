using home.api.Utilities;
using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs
{
    /// <summary>
    /// DTO de criação da família
    /// </summary>
    public class FamilyRequest
    {
        #region Properties

        [Required(ErrorMessage = "Campo obrigatório.")]
        [Length(2, 50, ErrorMessage = "Quantidade entre 2 e 50 caracteres.")]
        [RegularExpression(Global.REGEX_DEFAULT, ErrorMessage = "Insira caracteres válidos.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Demais membros da família. Quem cria já entra como membro automaticamente.
        /// </summary>
        public ICollection<Guid>? Members { get; set; }

        #endregion
    }
}
