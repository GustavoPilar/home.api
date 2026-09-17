using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    /// <summary>
    /// Usuário da aplicação, estendendo o Identity
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        #region Properties

        /// <summary>
        /// Primeiro nome
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Sobrenome
        /// </summary>
        public string? LastName { get; set; }

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

        #region Navigation

        [JsonIgnore]
        public ICollection<Home>? Homes { get; set; }

        /// <summary>
        /// Associações do usuário com famílias.
        /// Não existe navegação direta para Family: a relação é sempre
        /// explícita por UserFamily, para não gerar duas relações concorrentes.
        /// </summary>
        [JsonIgnore]
        public ICollection<UserFamily>? UserFamilies { get; set; }

        #endregion
    }
}
