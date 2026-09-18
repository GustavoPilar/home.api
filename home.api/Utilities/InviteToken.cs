using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;

namespace home.api.Utilities
{
    /// <summary>
    /// Geração e verificação do token de convite
    /// </summary>
    public static class InviteToken
    {
        #region Constants

        /// <summary>
        /// 32 bytes de entropia: inviável de adivinhar por força bruta
        /// </summary>
        public const int TOKEN_SIZE_IN_BYTES = 32;

        #endregion

        #region Members :: Generate(), Hash()

        /// <summary>
        /// Gera um token aleatório seguro, pronto para trafegar em URL.
        /// Não se usa Guid aqui: o Guid da versão 7 é sequencial no tempo e
        /// nenhuma versão foi projetada para ser segredo.
        /// </summary>
        public static string Generate()
        {
            byte[] buffer = RandomNumberGenerator.GetBytes(TOKEN_SIZE_IN_BYTES);

            return Base64Url.EncodeToString(buffer);
        }

        /// <summary>
        /// Calcula o hash do token para comparação e armazenamento.
        /// SHA-256 puro basta, sem alongamento: diferente de senha, o token já
        /// tem entropia máxima e não é vulnerável a dicionário.
        /// </summary>
        /// <param name="token">Token em claro</param>
        /// <exception cref="ArgumentException">Token vazio</exception>
        public static string Hash(string token)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));

            return Convert.ToBase64String(hash);
        }

        #endregion
    }
}
