﻿using System.Linq;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents the text of a Ruby annotation.
    /// </summary>
    public class HtmlElementTextSemanticsRt : HtmlElement, IHtmlElementTextSemantics
    {
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
        public HtmlElementTextSemanticsRt()
            : base("rt")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementTextSemanticsRt(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementTextSemanticsRt(params IHtmlNode[] nodes)
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
