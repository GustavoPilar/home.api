namespace home.api.Domain.Interfaces.Entities
{
    public interface IUser
    {
        #region Fields

        string FirstName { get; set; }

        string? LastName { get; set; }
        
        DateTime? Birthday { get; set; }

        bool Active { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }

        #endregion
    }
}
