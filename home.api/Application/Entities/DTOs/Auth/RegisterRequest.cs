using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs.Auth
{
    public class RegisterRequest
    {
        #region Fields

        [Required(ErrorMessage = "Campo obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Compare("Email", ErrorMessage = "E-mails não coincidem")]
        public string ConfirmEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(10, ErrorMessage = "Minimo de 10 caracteres.")]
        [MaxLength(20, ErrorMessage = "máximo de 20 caracteres.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(3, ErrorMessage = "Minimo de 3 caracteres.")]
        [MaxLength(50, ErrorMessage = "máximo de 50 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(2, ErrorMessage = "Minimo de 2 caracteres.")]
        [MaxLength(30, ErrorMessage = "máximo de 30 caracteres.")]
        public string? LastName { get; set; } = string.Empty;

        public DateTime? Birthday { get; set; }

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(8, ErrorMessage = "Minimo de 8 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo obrigatório.")]
        [MinLength(8, ErrorMessage = "Minimo de 8 caracteres.")]
        [Compare("Password", ErrorMessage = "Senhas não coincidem")]
        public string ConfirmPassword { get; set; } = string.Empty;

        #endregion
    }
}
