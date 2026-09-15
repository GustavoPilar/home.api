using home.api.Domain.Interfaces.Entities;

namespace home.api.Domain.Entities
{
    public abstract class EntityBase : IEntityBase
    {
        #region Fields

        /// <summary>
        /// Identify
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// When was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// When was updated
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
