using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the footer of a page or section. It often contains copyright notices, a link to the imprint or contact addresses.
    /// </summary>
    public class HtmlElementSectionFooter : HtmlElement, IHtmlElementSection
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
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementSectionFooter()
            : base("footer")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementSectionFooter(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementSectionFooter(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementSectionFooter(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }
    }
}
