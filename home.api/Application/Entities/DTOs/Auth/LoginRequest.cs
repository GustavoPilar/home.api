using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs.Auth
{
    public class LoginRequest
    {
        #region Fields

        [Required(ErrorMessage = "Campo obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(8, ErrorMessage = "Mínimo 8 caracteres.")]
        public string Password { get; set; } = string.Empty;

        #endregion
    }
}
