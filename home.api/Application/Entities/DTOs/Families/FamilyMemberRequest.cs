using home.api.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// Membro informado pelo host ao compor a família
    /// </summary>
    public class FamilyMemberRequest
    {
        #region Properties

        [Required(ErrorMessage = "Campo obrigatório.")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Título de parentesco. Nulo deixa o membro sem rótulo.
        /// </summary>
        public Guid? FamilyTitleId { get; set; }

        /// <summary>
        /// Papel de permissão. Omitido, entra como membro comum.
        /// </summary>
        public MembershipRole Role { get; set; } = MembershipRole.Member;

        #endregion
    }
}
