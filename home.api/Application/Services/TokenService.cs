using home.api.Application.Entities;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace home.api.Application.Services
{
    public class TokenService(
        IOptions<JwtSettings> jwtSettings) : ITokenService
    {
        #region Properties

        private readonly JwtSettings jwtSettings = jwtSettings.Value;

        #endregion

        #region Members

        public string GetToken(User user)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(this.jwtSettings.SecretKey));

            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            DateTime expiration = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes);

            JwtSecurityToken token = new(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: this.GetClaims(user),
                expires: expiration,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private List<Claim> GetClaims(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.Email ?? string.Empty),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return claims;
        }

        #endregion
    }
}
