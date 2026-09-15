namespace home.api.Domain.Interfaces.Entities
{
    public interface IEntityBase
    {
        #region Fields

        Guid Id { get; set; }

        DateTime CreatedAt { get; set; }

        DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
