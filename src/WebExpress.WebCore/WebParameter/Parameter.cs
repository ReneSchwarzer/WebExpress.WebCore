using System;
using System.Collections.Generic;
using System.Text;
using WebExpress.WebCore.WebIcon;
using WebExpress.WebCore.WebPage;

namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a parameter with a key, value, and scope.
    /// </summary>
    public class Parameter : IParameterDynamic
    {
        /// <summary>
        /// Returns the key of the parameter.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Returns or sets the scope of the parameter.
        /// </summary>
        public ParameterScope Scope { get; set; }

        /// <summary>
        /// Returns the value of the parameter.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public Parameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope of the parameter.</param>
        public Parameter(string key, Guid value, ParameterScope scope)
        {
            Key = key.ToLower();
            Value = value.ToString();
            Scope = scope;

            if (scope == ParameterScope.Parameter)
            {
                var decode = System.Web.HttpUtility.UrlDecode(value.ToString());
                Value = decode;
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope of the parameter.</param>
        public Parameter(string key, string value, ParameterScope scope)
        {
            Key = key.ToLower();
            Value = value;
            Scope = scope;

            if (scope == ParameterScope.Parameter)
            {
                var decode = System.Web.HttpUtility.UrlDecode(value);
                Value = decode;
            }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope of the parameter.</param>
        public Parameter(string key, int value, ParameterScope scope)
        {
            Key = key.ToLower();
            Value = value.ToString();
            Scope = scope;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="scope">The scope of the parameter.</param>
        public Parameter(string key, char value, ParameterScope scope)
        {
            Key = key.ToLower();
            Value = value.ToString();
            Scope = scope;
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
            return Value;
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
            return null;
        }

        /// <summary>
        /// Creates a parameter list.
        /// </summary>
        /// <param name="param">The elements of the parameter list.</param>
        /// <returns>The parameter list.</returns>
        public static List<Parameter> Create(params Parameter[] param)
        {
            return [.. param];
        }

        /// <summary>
        /// Conversion to string form.
        /// </summary>
        /// <returns>The object in its string representation.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder(Value);

            sb.Replace("%", "%25"); // Attention! & must come first
            sb.Replace(" ", "%20");
            sb.Replace("!", "%21");
            sb.Replace("\"", "%22");
            sb.Replace("#", "%23");
            sb.Replace("$", "%24");
            sb.Replace("&", "%26");
            sb.Replace("'", "%27");
            sb.Replace("(", "%28");
            sb.Replace(")", "%29");
            sb.Replace("*", "%2A");
            sb.Replace("+", "%2B");
            sb.Replace(",", "%2C");
            sb.Replace("-", "%2D");
            sb.Replace(".", "%2E");
            sb.Replace("/", "%2F");
            sb.Replace(":", "%3A");
            sb.Replace(";", "%3B");
            sb.Replace("<", "%3C");
            sb.Replace("=", "%3D");
            sb.Replace(">", "%3E");
            sb.Replace("?", "%3F");
            sb.Replace("@", "%40");
            sb.Replace("[", "%5B");
            sb.Replace("\\", "%5C");
            sb.Replace("]", "%5D");
            sb.Replace("{", "%7B");
            sb.Replace("|", "%7C");
            sb.Replace("}", "%7D");

            return string.Format("{0}={1}", Key, sb.ToString());
        }
    }
}
