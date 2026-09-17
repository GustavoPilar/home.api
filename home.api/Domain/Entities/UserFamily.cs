using home.api.Domain.Entities.Base;
using home.api.Domain.Enums;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    /// <summary>
    /// Associação entre um usuário e uma família.
    /// Carrega os dois atributos que só fazem sentido nesta relação:
    /// o título de parentesco e o papel de permissão.
    /// </summary>
    public class UserFamily : OwnedEntityBase
    {
        #region Properties

        /// <summary>
        /// Família à qual o usuário está associado
        /// </summary>
        public Guid FamilyId { get; set; }

        /// <summary>
        /// Título de parentesco do membro nesta família
        /// </summary>
        public Guid? FamilyTitleId { get; set; }

        /// <summary>
        /// Papel de permissão do membro nesta família
        /// </summary>
        public MembershipRole Role { get; set; } = MembershipRole.Member;

        #endregion

        #region Navigation

        [JsonIgnore]
        public Family? Family { get; set; }

        [JsonIgnore]
        public FamilyTitle? FamilyTitle { get; set; }

        #endregion
    }
}
