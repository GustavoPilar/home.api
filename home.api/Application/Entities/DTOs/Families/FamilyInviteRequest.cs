using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// Emissão de um convite. Sem e-mail é um link aberto;
    /// com e-mail, só o destinatário consegue aceitar.
    /// </summary>
    public class FamilyInviteRequest
    {
        #region Properties

        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(256, ErrorMessage = "Máximo de 256 caracteres.")]
        public string? TargetEmail { get; set; }

        #endregion
    }
}
