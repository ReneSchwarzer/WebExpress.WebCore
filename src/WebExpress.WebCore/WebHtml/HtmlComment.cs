﻿using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    public class HtmlComment : IHtmlNode
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlComment()
        {

        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="text">The text.</param>
        public HtmlComment(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public virtual void ToString(StringBuilder builder, int deep)
        {
            builder.Append("<!-- ");
            builder.Append(Text);
            builder.Append(" -->");
        }
    }
}
