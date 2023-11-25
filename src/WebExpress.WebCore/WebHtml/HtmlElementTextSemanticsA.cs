using System;
using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a hyperlink that points to another resource.
    /// </summary>
    public class HtmlElementTextSemanticsA : HtmlElement, IHtmlElementTextSemantics
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Text
        {
            get => string.Join("", Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Elements.Clear(); Elements.Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Returns or sets the alternate text.
        /// </summary>
        public string Alt
        {
            get => GetAttribute("alt");
            set => SetAttribute("alt", value);
        }

        /// <summary>
        /// Returns or sets the tooltip.
        /// </summary>
        public string Title
        {
            get => GetAttribute("title");
            set => SetAttribute("title", value);
        }

        /// <summary>
        /// Returns or sets the destination uri.
        /// </summary>
        public string Href
        {
            get => GetAttribute("href");
            set => SetAttribute("href", value);
        }

        /// <summary>
        /// Returns or sets the target.
        /// </summary>
        public TypeTarget Target
        {
            get => (TypeTarget)Enum.Parse(typeof(TypeTarget), GetAttribute("target"));
            set => SetAttribute("target", value.ToStringValue());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTextSemanticsA()
            : base("a")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementTextSemanticsA(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsA(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsA(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }
    }
}
