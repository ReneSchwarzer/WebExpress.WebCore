using System.Collections.Generic;
using System.Text;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents a form. Forms typically consist of a set of controls, the values of which are 
    /// transmitted to a server for further processing.
    /// </summary>
    public class HtmlElementFormForm : HtmlElement, IHtmlElementForm
    {
        /// <summary>
        /// Returns or sets the name of the form.
        /// </summary>
        public string Name
        {
            get => GetAttribute("name");
            set => SetAttribute("name", value);
        }

        /// <summary>
        /// Returns or sets the character encoding.
        /// </summary>
        public string AcceptCharset
        {
            get => GetAttribute("accept-charset");
            set => SetAttribute("accept-charset", value);
        }

        /// <summary>
        /// Returns or sets the character encoding.
        /// </summary>
        public TypeEnctype Enctype
        {
            get => TypeEnctypeExtensions.Convert(GetAttribute("enctype"));
            set => SetAttribute("enctype", value.Convert());
        }

        /// <summary>
        /// Returns or sets the method post or get.
        /// </summary>
        public string Method
        {
            get => GetAttribute("method");
            set => SetAttribute("method", value);
        }

        /// <summary>
        /// Returns or sets the uri.
        /// </summary>
        public string Action
        {
            get => GetAttribute("action");
            set => SetAttribute("action", value);
        }

        /// <summary>
        /// Returns or sets the target window.
        /// </summary>
        public string Target
        {
            get => GetAttribute("target");
            set => SetAttribute("target", value);
        }

        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementFormForm()
            : base("form")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementFormForm(string text)
            : this()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormForm(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            base.ToString(builder, deep);
        }
    }
}
