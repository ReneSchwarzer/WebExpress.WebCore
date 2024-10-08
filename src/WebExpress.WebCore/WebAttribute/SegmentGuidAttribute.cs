using System;
using WebExpress.WebCore.WebMessage;
using WebExpress.WebCore.WebUri;

namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// A dynamic path segment of type guid.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SegmentGuidAttribute<T> : Attribute, IEndpointAttribute, ISegmentAttribute where T : Parameter
    {
        /// <summary>
        /// Returns or sets the name of the variable.
        /// </summary>
        private string VariableName { get; set; }

        /// <summary>
        /// Returns or sets the display string.
        /// </summary>
        private string Display { get; set; }

        /// <summary>
        /// Returns or sets the display format.
        /// </summary>
        private UriPathSegmentVariableGuid.Format DisplayFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="parameter">The type of the variable.</param>
        /// <param name="display">The display string.</param>
        /// <param name="displayFormat">The display format.</param>
        public SegmentGuidAttribute(string display, UriPathSegmentVariableGuid.Format displayFormat = UriPathSegmentVariableGuid.Format.Simple)
        {
            VariableName = (Activator.CreateInstance(typeof(T)) as Parameter)?.Key?.ToLower();
            Display = display;
            DisplayFormat = displayFormat;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            return new UriPathSegmentVariableGuid(VariableName, Display, DisplayFormat);
        }
    }
}
