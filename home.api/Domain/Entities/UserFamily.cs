using home.api.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    /// <summary>
    /// Associação entre um usuário e uma família
    /// </summary>
    public class UserFamily : OwnedEntityBase
    {
        #region Properties

        /// <summary>
        /// Família à qual o usuário está associado
        /// </summary>
        public Guid FamilyId { get; set; }

        #endregion

        #region Navigation

        [JsonIgnore]
        public Family? Family { get; set; }

        #endregion
    }
}
