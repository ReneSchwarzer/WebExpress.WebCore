using System.Collections.Generic;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a set of controls.
    /// </summary>
    public class HtmlElementFormFieldset : HtmlElement, IHtmlElementFormItem
    {
        /// <summary>
        /// Returns or sets the name of the input field.
        /// </summary>
        public string Name
        {
            get => GetAttribute("name");
            set => SetAttribute("name", value);
        }


        /// <summary>
        /// Liefert oder setzt die Label-Eigenschaft
        /// </summary>
        public bool Disable
        {
            get => HasAttribute("disabled");
            set { if (value) { SetAttribute("disabled"); } else { RemoveAttribute("disabled"); } }
        }

        /// <summary>
        /// Returns or sets the identification name of the form element to which it is associated.
        /// </summary>
        public string Form
        {
            get => GetAttribute("form");
            set => SetAttribute("form", value);
        }

        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementFormFieldset()
            : base("fieldset")
        {
            CloseTag = false;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormFieldset(params IHtmlNode[] nodes)
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
