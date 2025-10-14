using WebExpress.WebCore.WebIdentity;

namespace WebExpress.WebCore.Test.Data
{
    /// <summary>
    /// Represents a group identity with an ID and a name.
    /// </summary>
    internal class MockIdentityGroup : IIdentityGroup
    {
        private readonly List<string> _roles = [];

        /// <summary>
        /// Returns or sets the id of the group.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Returns or sets the name of the group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns the roles associated with the group.
        /// </summary>
        public IEnumerable<string> Policies => _roles;

        /// <summary>
        /// Initializes a new instance of the class with the specified id and name.
        /// </summary>
        /// <param name="id">The id of the group.</param>
        /// <param name="name">The name of the group.</param>
        public MockIdentityGroup(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Assigns roles.
        /// </summary>
        /// <param name="roles">The list of roles to assign group to.</param>
        public void Assign(IEnumerable<string> roles)
        {
            _roles.AddRange(roles);
        }
    }
}
