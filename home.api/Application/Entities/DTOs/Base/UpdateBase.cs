using home.api.Utilities;

namespace home.api.Application.Entities.DTOs.Base
{
    /// <summary>
    /// Base dos DTOs de atualização
    /// </summary>
    public class UpdateBase : IUpdateBase
    {
        #region Properties

        /// <summary>
        /// Identificador da entidade que será alterada
        /// </summary>
        [NotEmptyGuid(ErrorMessage = "Campo obrigatório.")]
        public Guid Id { get; set; }

        #endregion
    }
}
