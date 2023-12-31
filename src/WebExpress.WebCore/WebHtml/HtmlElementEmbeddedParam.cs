﻿namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a parameter for a plugin that can be used for the display 
    /// of an <object> embedded element.
    /// </summary>
    public class HtmlElementEmbeddedParam : HtmlElement, IHtmlElementEmbedded
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementEmbeddedParam()
            : base("param", false)
        {

        }
    }
}
