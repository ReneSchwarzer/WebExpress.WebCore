using System.Collections.Generic;
using System.Text;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents a control that can be used to select from a range of options.
    /// <select name="top5" size="2">
    ///  <option>Michael Jackson</option>
    ///  <option selected>Tom Waits</option>
    /// </select>
    /// </summary>
    public class HtmlElementFieldSelect : HtmlElement, IHtmlFormularItem
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Returns or sets the name of the input field.
        /// </summary>
        public string Name
        {
            get => GetAttribute("name");
            set => SetAttribute("name", value);
        }

        /// <summary>
        /// Liefert oder setzt die Anzahl der gleichzeitig sichtbaren Elemente der Auswahlliste
        /// </summary>
        public string Size
        {
            get => GetAttribute("size");
            set => SetAttribute("size", value);
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
        /// Liefert oder setzt ob das Eingabefeld verwendet werden kann
        /// </summary>
        public bool Disabled
        {
            get => HasAttribute("disabled");
            set { if (value) { SetAttribute("disabled"); } else { RemoveAttribute("disabled"); } }
        }

        /// <summary>
        /// Returns or sets the OnChange attribute.
        /// </summary>
        public string OnChange
        {
            get => GetAttribute("onchange");
            set => SetAttribute("onchange", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementFieldSelect()
            : base("select")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFieldSelect(params IHtmlNode[] nodes)
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
