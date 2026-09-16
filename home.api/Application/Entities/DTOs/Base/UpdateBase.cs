using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Campo obrigatório.")]
        public Guid Id { get; set; }

        #endregion
    }
}
