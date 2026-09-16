namespace home.api.Application.Entities
{
    public class JwtSettings
    {
        #region Fields

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public int ExpirationInMinutes { get; set; }

        #endregion
    }
}
