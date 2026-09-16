namespace home.api.Application.Entities.DTOs.Base
{
    /// <summary>
    /// DTO de saída sem dependência do Domain: quem preenche estes campos é o mapeador
    /// </summary>
    public class ResponseBase : IResponseBase
    {
        #region Properties

        /// <summary>
        /// Identificador da entidade
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Usuário proprietário da entidade
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Data de criação
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data da última alteração
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
