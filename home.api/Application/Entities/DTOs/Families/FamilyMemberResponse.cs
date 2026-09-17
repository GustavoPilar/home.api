using home.api.Domain.Enums;

namespace home.api.Application.Entities.DTOs.Families
{
    /// <summary>
    /// Membro devolvido na leitura da família
    /// </summary>
    public class FamilyMemberResponse
    {
        #region Properties

        public Guid UserId { get; set; }

        public Guid? FamilyTitleId { get; set; }

        /// <summary>
        /// Nome do título já resolvido, para o cliente não precisar de uma segunda chamada
        /// </summary>
        public string? FamilyTitleName { get; set; }

        public MembershipRole Role { get; set; }

        #endregion
    }
}
