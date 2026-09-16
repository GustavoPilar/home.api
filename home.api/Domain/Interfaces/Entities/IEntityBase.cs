using home.api.Domain.Entities;

namespace home.api.Domain.Interfaces.Entities
{
    public interface IEntityBase
    {
        #region Fields

        Guid Id { get; set; }

        Guid UserId { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? LastUpdatedAt { get; set; }

        #endregion

        #region Navigation

        User? User { get; set; }

        #endregion
    }
}
