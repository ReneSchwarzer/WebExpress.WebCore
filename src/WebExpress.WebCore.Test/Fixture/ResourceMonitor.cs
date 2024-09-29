using WebExpress.WebCore.WebResource;

namespace WebExpress.WebCore.Test.Fixture
{
    /// <summary>
    /// The class manages the component managers and disposes them when the <c>using</c> statement is exited.
    /// </summary>
    internal class ResourceMonitor : IDisposable
    {
        /// <summary>
        /// Returns the number of resources.
        /// </summary>
        public int Count => ResourceCounter.Resources.Count();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMonitor"/> class with the specified component manager.
        /// </summary>
        public ResourceMonitor()
        {
            ResourceCounter.Clear();
        }

        /// <summary>
        /// Checks if a resource of the same type exists.
        /// </summary>
        /// <param name="type">The type of resource to check for.</param>
        /// <returns><c>true</c> if a resource of the same type exists; otherwise, <c>false</c>.</returns>
        public bool Contains(Type type)
        {
            return ResourceCounter.Resources.Any(resource => resource.Equals(type));
        }

        /// <summary>
        /// Checks if a resource of the same type exists.
        /// </summary>
        /// <typeparam name="T">The type of resource to check for.</typeparam>
        /// <returns><c>true</c> if a resource of the same type exists; otherwise, <c>false</c>.</returns>
        public bool Contains<T>() where T : IResource
        {
            return ResourceCounter.Resources.Any(resource => resource is T);
        }

        /// <summary>
        /// Disposes all resources.
        /// </summary>
        public void Dispose()
        {
            ResourceCounter.Clear();
        }
    }
}
