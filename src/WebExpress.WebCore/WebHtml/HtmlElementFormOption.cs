using System.Linq;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a selection option within an select element, or a suggestion within an datalist element.
    /// </summary>
    /// <code>
    /// <select name="top5" size="5">
    ///   <option>Michael Jackson</option>
    ///   <option selected="">Tom Waits</option>
    /// </select>
    /// </code>
    public class HtmlElementFormOption : HtmlElement, IHtmlElementFormItem
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text
        {
            get => string.Join("", Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Clear(); Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Gets or sets a value.
        /// </summary>
        public string Value
        {
            get => GetAttribute("value");
            set => SetAttribute("value", value);
        }

        /// <summary>
        /// Gets or sets whether the field is selected.
        /// </summary>
        public bool Selected
        {
            get => HasAttribute("selected");
            set { if (value) { SetAttribute("selected"); } else { RemoveAttribute("selected"); } }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the option is disabled.
        /// </summary>
        public bool Disabled
        {
            get => HasAttribute("disabled");
            set { if (value) { SetAttribute("disabled"); } else { RemoveAttribute("disabled"); } }
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementFormOption()
            : base("option")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormOption(params IHtmlNode[] nodes)
            : this()
        {
            Add(nodes);
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
