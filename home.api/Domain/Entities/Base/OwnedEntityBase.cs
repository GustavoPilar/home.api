using home.api.Domain.Interfaces.Entities;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities.Base
{
    /// <summary>
    /// Base das entidades que pertencem a um usuário
    /// </summary>
    public abstract class OwnedEntityBase : EntityBase, IOwnedEntity
    {
        #region Properties

        /// <summary>
        /// Usuário proprietário da entidade
        /// </summary>
        public Guid UserId { get; set; }

        #endregion

        #region Navigation

        [JsonIgnore]
        public User? User { get; set; }

        #endregion
    }
}
