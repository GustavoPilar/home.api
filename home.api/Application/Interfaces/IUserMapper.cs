using home.api.Application.Entities.DTOs;
using home.api.Domain.Entities;

namespace home.api.Application.Interfaces
{
    /// <summary>
    /// Contrato de conversão do usuário do Identity para o seu DTO de saída.
    /// O usuário não herda de EntityBase, por isso possui um mapeador próprio.
    /// </summary>
    public interface IUserMapper
    {
        #region Members :: ToResponse()

        /// <summary>
        /// Converte o usuário no DTO de saída
        /// </summary>
        /// <param name="user">Usuário do Identity</param>
        UserResponse ToResponse(User user);

        #endregion
    }
}
