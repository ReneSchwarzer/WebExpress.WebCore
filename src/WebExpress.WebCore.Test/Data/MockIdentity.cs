using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test.Data
{
    /// <summary>
    /// Represents an identity object that implements the IIdentity interface.
    /// </summary>
    internal class MockIdentity : IIdentity
    {
        private readonly List<IIdentityGroup> _groups = new();

        /// <summary>
        /// Returns or sets the id of the user.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Returns or sets the name of the user.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns or sets the email of the user.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Returns the hash of the password.
        /// </summary>
        public string PasswordHash { get; }

        /// <summary>
        /// Returns the groups associated with the user.
        /// </summary>
        public IEnumerable<IIdentityGroup> Groups => _groups;

        /// <summary>
        /// Initializes a new instance of the class with the specified id, name, email, and password.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <param name="name">The name of the user.</param>
        /// <param name="email">The email of the user.</param>
        /// <param name="passwordHash">The password hash of the user.</param>
        public MockIdentity(Guid id, string name, string email, string passwordHash)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// Assigns groups.
        /// </summary>
        /// <param name="groups">The list of groups to assign users to.</param>
        public void Assign(IEnumerable<IIdentityGroup> groups)
        {
            _groups.AddRange(groups);
        }


    }
}
