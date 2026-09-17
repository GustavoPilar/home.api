using home.api.Application.Entities.DTOs.Base;
using home.api.Utilities;
using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs.Families
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
        /// Composição completa da família. Nula mantém os membros atuais;
        /// preenchida, substitui presença, título e papel de cada um.
        /// </summary>
        public ICollection<FamilyMemberRequest>? Members { get; set; }

        #endregion
    }
}
