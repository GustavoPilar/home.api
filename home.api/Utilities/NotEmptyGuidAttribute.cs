using System.ComponentModel.DataAnnotations;

namespace home.api.Utilities
{
    /// <summary>
    /// Exige um Guid preenchido.
    /// [Required] não serve para Guid não anulável: o valor ausente vira
    /// Guid.Empty, que a anotação considera preenchido, e o campo passa batido.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        #region Constructors

        public NotEmptyGuidAttribute() : base("Campo obrigatório.")
        {
        }

        #endregion

        #region Members :: IsValid()

        /// <summary>
        /// Considera válido apenas um Guid diferente de Guid.Empty
        /// </summary>
        /// <param name="value">Valor informado</param>
        public override bool IsValid(object? value)
        {
            if (value is Guid guid)
                return guid != Guid.Empty;

            return false;
        }

        #endregion
    }
}
