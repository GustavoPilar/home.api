using home.api.Domain.Entities;

namespace home.api.Application.Interfaces
{
    public interface ITokenService
    {
        #region Fields

        string GetToken(User user);

        #endregion
    }
}
