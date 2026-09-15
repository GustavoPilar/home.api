using home.api.Domain.Entities;

namespace home.api.Application.Entities.DTOs.User
{
    public class UserResponse(Domain.Entities.User user)
    {
        #region Fields

        /// <summary>
        /// Identity
        /// </summary>
        public Guid Id { get; set; } = user.Id;

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; } = user.FirstName;

        /// <summary>
        /// User's surname
        /// </summary>
        public string? LastName { get; set; } = user.LastName;

        /// <summary>
        /// User's social name
        /// </summary>
        public string UserName { get; set; } = user.UserName;

        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; } = user.Email;

        /// <summary>
        /// User's birthday
        /// </summary>
        public DateTime? Birthday { get; set; } = user.Birthday;

        /// <summary>
        /// User is active?
        /// </summary>
        public bool Active { get; set; } = user.Active;

        /// <summary>
        /// When was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = user.CreatedAt;

        /// <summary>
        /// When was updated
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; } = user.LastUpdatedAt;

        #endregion
    }
}
