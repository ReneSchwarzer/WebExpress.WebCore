using System;

namespace WebExpress.WebComponent
{
    public class ComponentItem
    {
        /// <summary>
        /// Returns or set the class type for a component.
        /// </summary>
        public Type ComponentClass { get; internal set; }

        /// <summary>
        /// Returns the component id.
        /// </summary>
        public string ComponentId { get; internal set; }

        /// <summary>
        /// Returns the component instance or null if not already created.
        /// </summary>
        public IComponent ComponentInstance { get; internal set; }

        /// <summary>
        /// Constructor
        /// </summary>
        internal ComponentItem()
        {

        }
    }
}
