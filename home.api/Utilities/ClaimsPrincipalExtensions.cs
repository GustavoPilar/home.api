using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace home.api.Utilities
{
    /// <summary>
    /// Extensões de leitura das claims do usuário autenticado
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        #region Members :: GetUserId()

        /// <summary>
        /// Extrai o identificador do usuário a partir do token.
        /// A origem do identificador é sempre o token, nunca o corpo da requisição.
        /// </summary>
        /// <param name="principal">Identidade da requisição</param>
        /// <exception cref="ArgumentNullException">Identidade nula</exception>
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            // O handler do JWT pode ou não mapear "sub" para NameIdentifier, então as duas claims são consultadas
            string? claimValue = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

            return Guid.TryParse(claimValue, out Guid userId) ? userId : Guid.Empty;
        }

        /// <summary>
        /// Extrai o e-mail do usuário a partir do token, para validar
        /// convites dirigidos sem uma ida extra ao banco
        /// </summary>
        /// <param name="principal">Identidade da requisição</param>
        /// <exception cref="ArgumentNullException">Identidade nula</exception>
        public static string? GetUserEmail(this ClaimsPrincipal principal)
        {
            ArgumentNullException.ThrowIfNull(principal);

            return principal.FindFirstValue(ClaimTypes.Email)
                ?? principal.FindFirstValue(ClaimTypes.Name)
                ?? principal.FindFirstValue(JwtRegisteredClaimNames.UniqueName);
        }

        #endregion
    }
}
