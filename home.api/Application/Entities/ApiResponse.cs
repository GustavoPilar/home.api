namespace home.api.Application.Entities
{
    /// <summary>
    /// Envelope único de resposta da API
    /// </summary>
    /// <typeparam name="T">Conteúdo devolvido ao cliente</typeparam>
    public class ApiResponse<T>
        where T : class
    {
        #region Properties

        /// <summary>
        /// Indica se a operação foi concluída
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// Mensagem destinada ao usuário final
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Conteúdo da resposta
        /// </summary>
        public T? Data { get; set; }

        #endregion
    }
}
