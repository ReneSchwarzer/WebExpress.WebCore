using System.Collections.Generic;
using System.Text;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents a set of logically grouped selection options.
    /// <select name="top5" size="5">
    ///  <optgroup label="Names with A">
    ///   <option value="1">Michael Jackson</option>
    ///   <option value="2" selected>Tom Waits</option>
    ///  </optgroup>
    /// </select>
    /// </summary>
    public class HtmlElementFormOptgroup : HtmlElement, IHtmlFormularItem
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Returns or sets the label.
        /// </summary>
        public string Label
        {
            get => GetAttribute("label");
            set => SetAttribute("label", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementFormOptgroup()
            : base("optgroup")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormOptgroup(params IHtmlNode[] nodes)
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
