using home.api.Domain.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace home.api.Domain.Entities
{
    public class User : IdentityUser<Guid>, IUser
    {
        #region Fields

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// User's surname
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// User's birthday
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Users is active?
        /// </summary>
        public bool Active { get; set; }

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
