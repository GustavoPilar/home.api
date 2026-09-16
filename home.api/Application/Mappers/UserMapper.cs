using home.api.Application.Entities.DTOs;
using home.api.Application.Interfaces;
using home.api.Domain.Entities;

namespace home.api.Application.Mappers
{
    /// <summary>
    /// Mapeamento do usuário do Identity para o seu DTO de saída
    /// </summary>
    public class UserMapper : IUserMapper
    {
        #region Members :: ToResponse()

        /// <summary>
        /// Converte o usuário no DTO de saída, sem expor hash de senha nem tokens de segurança
        /// </summary>
        /// <param name="user">Usuário do Identity</param>
        /// <exception cref="ArgumentNullException">Usuário nulo</exception>
        public UserResponse ToResponse(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            return new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                Birthday = user.Birthday,
                Active = user.Active,
                CreatedAt = user.CreatedAt,
                LastUpdatedAt = user.LastUpdatedAt
            };
        }

        #endregion
    }
}
