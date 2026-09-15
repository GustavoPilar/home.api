using home.api.Application.Entities.DTOs.User;

namespace home.api.Application.Entities.DTOs.auth
{
    public class RegisterResponse
    {
        #region Fields

        public bool IsSucceeded { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public UserResponse? User { get; set; }

        #endregion
    }
}
