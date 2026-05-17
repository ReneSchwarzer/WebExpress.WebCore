using System;

namespace WebExpress.WebCore.WebComponent.Model
{
    /// <summary>
    /// Represents an item of a web component, including its class type, ID, and instance.
    /// </summary>
    public class ComponentItem
    {
        /// <summary>
        /// Gets or set the class type for a component.
        /// </summary>
        public Type ComponentClass { get; internal set; }

        /// <summary>
        /// Gets the component id.
        /// </summary>
        public string ComponentId { get; internal set; }

        /// <summary>
        /// Gets the component instance or null if not already created.
        /// </summary>
        public IComponentManager ComponentInstance { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        internal ComponentItem()
        {

        }
    }
}
