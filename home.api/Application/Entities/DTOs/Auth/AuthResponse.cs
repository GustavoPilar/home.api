namespace home.api.Application.Entities.DTOs.Auth
{
    /// <summary>
    /// Conteúdo devolvido ao autenticar: o usuário e o token de acesso
    /// </summary>
    public class AuthResponse
    {
        #region Properties

        public UserResponse? User { get; set; }

        public string Token { get; set; } = string.Empty;

        #endregion
    }
}
