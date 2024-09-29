using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test.Fixture
{
    /// <summary>
    /// The class manages a list of <see cref="IResource"/> objects.
    /// </summary>
    internal static class ResourceCounter
    {
        private static List<IResource> _resources = new List<IResource>();

        /// <summary>
        /// Returns the number of resources.
        /// </summary>
        public static int Count => ResourceCounter.Resources.Count();

        /// <summary>
        /// Returns the component manager.
        /// </summary>
        public static IEnumerable<IResource> Resources => _resources;

        /// <summary>
        /// Adds a resource to the monitor.
        /// </summary>
        /// <param name="resource">The resource to be added.</param>
        public static void Add(IResource resource)
        {
            _resources.Add(resource);
        }

        /// <summary>
        /// Checks if a resource of the same type exists.
        /// </summary>
        /// <typeparam name="T">The type of resource to check for.</typeparam>
        /// <returns><c>true</c> if a resource of the same type exists; otherwise, <c>false</c>.</returns>
        public static bool Contains<T>() where T : IResource
        {
            return _resources.Any(resource => resource is T);
        }

        /// <summary>
        /// Clears the list of resources.
        /// </summary>
        public static void Clear()
        {
            _resources.Clear();
        }
    }
}
