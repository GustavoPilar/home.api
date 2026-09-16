namespace home.api.Application.Entities.DTOs
{
    /// <summary>
    /// DTO de saída do usuário
    /// </summary>
    public class UserResponse
    {
        #region Properties

        /// <summary>
        /// Identificador do usuário
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Primeiro nome
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Sobrenome
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Nome de exibição
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Data de nascimento
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Usuário ativo?
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data da última alteração
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
