using System;
using WebExpress.Core.WebUri;

namespace WebExpress.Core.WebAttribute
{
    public class SegmentDoubleAttribute : Attribute, IResourceAttribute, ISegmentAttribute
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
        /// Constructor
        /// </summary>
        /// <param name="variableName">The name of the variable.</param>
        /// <param name="display">The display string.</param>
        public SegmentDoubleAttribute(string variableName, string display)
        {
            VariableName = variableName;
            Display = display;
        }

        /// <summary>
        /// Conversion to a path segment.
        /// </summary>
        /// <returns>The path segment.</returns>
        public IUriPathSegment ToPathSegment()
        {
            //var expression = @"^[+-]?(\d*,\d+|\d+(,\d*)?)( +[eE][+-]?\d+)?$";

            //var callBackDisplay = new Func<string, string, CultureInfo, string>((segment, moduleId, culture) =>
            //{
            //    return null;
            //});

            //var callBackValiables = new Func<string, IDictionary<string, string>>(segment =>
            //{
            //    return null;
            //});

            return new UriPathSegmentVariableDouble(VariableName, Display);
        }
    }
}
