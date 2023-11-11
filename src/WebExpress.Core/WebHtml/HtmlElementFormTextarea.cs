using System.Linq;
using System.Text;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents an element for multi-line text input.
    /// </summary>
    public class HtmlElementFormTextarea : HtmlElement, IHtmlFormularItem
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
        /// Returns or sets the value. des Eingabefeldes
        /// </summary>
        public string Value
        {
            get => string.Join(string.Empty, Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Elements.Clear(); Elements.Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Returns or sets the number of rows displayed.
        /// </summary>
        public string Rows
        {
            get => GetAttribute("rows");
            set => SetAttribute("rows", value);
        }

        /// <summary>
        /// Returns or sets the number of columns displayed.
        /// </summary>
        public string Cols
        {
            get => GetAttribute("cols");
            set => SetAttribute("cols", value);
        }

        /// <summary>
        /// Returns or sets whether the text should be wrapped.
        /// Possible values are: hard, soft.
        /// </summary>
        public string Wrap
        {
            get => GetAttribute("wrap");
            set => SetAttribute("wrap", value);
        }

        /// <summary>
        /// Returns or sets whether the field is read-only.
        /// </summary>
        public string Readonly
        {
            get => GetAttribute("readonly");
            set => SetAttribute("readonly", value);
        }

        /// <summary>
        /// Returns or sets the minimum length.
        /// </summary>
        public string MinLength
        {
            get => GetAttribute("minlength");
            set => SetAttribute("minlength", value);
        }

        /// <summary>
        /// Returns or sets the maximum length.
        /// </summary>
        public string MaxLength
        {
            get => GetAttribute("maxlength");
            set => SetAttribute("maxlength", value);
        }

        /// <summary>
        /// Returns or sets whether inputs are enforced.
        /// </summary>
        public bool Required
        {
            get => HasAttribute("required");
            set { if (value) { SetAttribute("required"); } else { RemoveAttribute("required"); } }
        }

        /// <summary>
        /// Returns or sets a placeholder text.
        /// </summary>
        public string Placeholder
        {
            get => GetAttribute("placeholder");
            set => SetAttribute("placeholder", value);
        }

        /// <summary>
        /// Returns or sets a search pattern that checks the content.
        /// </summary>
        public string Pattern
        {
            get => GetAttribute("pattern");
            set => SetAttribute("pattern", value);
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
        /// Constructor
        /// </summary>
        public HtmlElementFormTextarea()
            : base("textarea")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormTextarea(params IHtmlNode[] nodes)
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
