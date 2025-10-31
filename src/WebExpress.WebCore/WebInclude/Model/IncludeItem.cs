using System;
using System.Collections.Generic;
using WebExpress.WebCore.WebApplication;
using WebExpress.WebCore.WebComponent;
using WebExpress.WebCore.WebPlugin;

namespace WebExpress.WebCore.WebInclude.Model
{
    /// <summary>
    /// An include item represents a registered include with runtime instance storage (when cached).
    /// </summary>
    internal sealed class IncludeItem : IDisposable
    {
        private readonly IComponentHub _componentHub;

        /// <summary>
        /// Returns or sets the identifier of the included component.
        /// </summary>
        public ComponentId IncludeId { get; set; }

        /// <summary>
        /// Returns or sets the context for the plugin, providing access to shared resources and services.
        /// </summary>
        public IPluginContext PluginContext { get; set; }

        /// <summary>
        /// Returns or sets the application context that provides configuration and services for the application.
        /// </summary>
        public IApplicationContext ApplicationContext { get; set; }

        /// <summary>
        /// Returns or sets the context used to manage include operations for related entities.
        /// </summary>
        /// <remarks>
        /// This property is typically used to specify the context for including related entities
        /// in queries.
        /// </remarks>
        public IIncludeContext IncludeContext { get; set; }

        /// <summary>
        /// Type or sets the type of the class to include in the operation.
        /// </summary>
        /// <remarks>
        /// This property is typically used to specify the class type that should be included in
        /// a particular operation or process. Ensure that the specified type is compatible with the operation's
        /// requirements.
        /// </remarks>
        public Type IncludeClass { get; set; }

        /// <summary>
        /// Returns or sets a value indicating whether caching is enabled.
        /// </summary>
        public bool Cache { get; set; }

        /// <summary>
        /// Returns or sets the collection of scopes associated with the current context.
        /// </summary>
        /// <remarks>
        /// The collection can be empty if no scopes are defined. Callers can set this property
        /// to customize the applicable scopes or retrieve it to inspect the current configuration.
        /// </remarks>
        public IEnumerable<Type> Scopes { get; set; } = [];

        /// <summary>
        /// Returns or sets the collection of files to be included.
        /// </summary>
        public IEnumerable<IncludeFile> Files { get; set; } = [];

        /// <summary>
        /// Initializes a new instance of the class with the specified component hub.
        /// </summary>
        /// <param name="componentHub">
        /// The component hub used to manage and interact with components. This parameter cannot 
        /// be null.
        /// </param>
        public IncludeItem(IComponentHub componentHub)
        {
            _componentHub = componentHub;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the class.
        /// </summary>
        /// <remarks>This method is provided to support the <see cref="IDisposable"/> pattern. Currently,
        /// it does not release any unmanaged resources, but it is included  to allow for future 
        /// extensibility if unmanaged resources are added.</remarks>
        public void Dispose()
        {
            // no unmanaged resources to release yet
        }
    }
}
