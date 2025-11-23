using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebExpress.WebCore.Internationalization;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPage;
using WebExpress.WebCore.WebParameter;

namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// Variable path segment.
    /// </summary>
    /// <typeparam name="TParameter">The parameter type.</typeparam>
    public abstract class UriPathSegmentVariable<TParameter> : IUriPathSegmentVariable
        where TParameter : IParameter
    {
        /// <summary>
        /// Returns or sets the id.
        /// </summary>
        public string Id => VariableName?.ToLower();

        /// <summary>
        /// Returns or sets the variable name.
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// Returns or sets the path text.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Returns or sets the regex expression.
        /// </summary>
        public string Expression { get; protected set; }

        /// <summary>
        /// Returns or sets the tag.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Checks for empty path segment.
        /// </summary>
        public bool IsEmpty => string.IsNullOrWhiteSpace(VariableName) || VariableName.Equals("/");

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="tag">The tag or null</param>
        public UriPathSegmentVariable(string name, object tag = null)
        {
            VariableName = name;
            Tag = tag;
        }

        /// <summary>
        /// Returns the variable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The variable value pair.</returns>
        public abstract IDictionary<string, string> GetVariable(string value);

        /// <summary>
        /// Checks whether the node matches the path element.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns>True if the path element matched, false otherwise.</returns>
        public virtual bool IsMatched(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Expression) && Value.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (Regex.IsMatch(value, Expression, RegexOptions.IgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Make a deep copy.
        /// </summary>
        /// <returns>The copy.</returns>
        public abstract IUriPathSegment Copy();

        /// <summary>
        /// Compare the object.
        /// </summary>
        /// <param name="obj">The comparison object.</param>
        /// <returns>true if equals, false otherwise</returns>
        public virtual bool Equals(IUriPathSegment obj)
        {
            if (obj is null)
            {
                return false;
            }
            else if (obj is UriPathSegmentVariable<TParameter> segment)
            {
                return VariableName.Equals(segment.VariableName, StringComparison.OrdinalIgnoreCase)
                    && (
                        (Expression is null && segment.Expression is null)
                        || Expression.Equals(segment.Expression)
                    );
            }

            return false;
        }

        /// <summary>
        /// Creates a deep copy of the current path segment and assigns the specified value.
        /// </summary>
        /// <param name="value">
        /// The string value to assign to the copied segment.
        /// </param>
        /// <returns>
        /// A new instance representing the copied segment with the assigned value.
        /// </returns>
        public IUriPathSegment Copy(string value)
        {
            var copy = Copy();
            if (copy is UriPathSegmentVariable<TParameter> segment)
            {
                segment.Value = value;
            }

            return copy;
        }

        /// <summary>
        /// Returns a string that represents the display text for the current instance.
        /// </summary>
        /// <param name="renderContext">The render context.</param>
        /// <returns>
        /// A string containing the display text associated with the instance. The 
        /// value may be empty if no display text is available.
        /// </returns>
        public virtual string GetDisplayText(IRenderContext renderContext)
        {
            var parameter = renderContext.Request.GetParameter<TParameter>();
            var displayText = parameter.GetDisplayText(renderContext);

            return string.Format(I18N.Translate(renderContext, displayText), Value);
        }

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
        public virtual IIcon GetIcon(IRenderContext renderContext)
        {
            var parameter = renderContext.Request.GetParameter<TParameter>();
            var icon = parameter.GetIcon(renderContext);

            return icon;
        }

        /// <summary>
        /// Converts the segment to a string.
        /// </summary>
        /// <returns>A string that represents the current segment.</returns>
        public override string ToString()
        {
            return Value ?? $"${{{VariableName}}}";
        }
    }
}