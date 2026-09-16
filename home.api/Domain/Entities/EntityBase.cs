using home.api.Domain.Interfaces.Entities;
using System.Text.Json.Serialization;

namespace home.api.Domain.Entities
{
    public abstract class EntityBase : IEntityBase
    {
        #region Fields

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        #endregion

        #region Navigation

        [JsonIgnore]
        public User? User { get; set; }

        #endregion
    }
}
