using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a parameter with a key, value, and scope.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Returns the key of the parameter.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Returns or sets the scope of the parameter.
        /// </summary>
        ParameterScope Scope { get; internal set; }

        /// <summary>
        /// Returns the value of the parameter.
        /// </summary>
        string Value { get; internal set; }

        /// <summary>
        /// Returns a string that represents the display text for the current instance.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        /// <returns>
        /// A string containing the display text associated with the instance. The 
        /// value may be empty if no display text is available.
        /// </returns>
        string GetDisplayText(IRenderContext renderContext);

        /// <summary>
        /// Returns an icon that visually represents the parameter within the given render context.
        /// </summary>
        /// <param name="renderContext">
        /// The rendering context that provides information required to determine the appropriate icon.
        /// </param>
        /// <returns>
        /// An icon associated with the current instance. The value may be <c>null</c> or empty 
        /// if no icon is available.
        /// </returns>
        IIcon GetIcon(IRenderContext renderContext);
    }
}
