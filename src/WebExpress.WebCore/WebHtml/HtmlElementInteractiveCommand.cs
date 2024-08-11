﻿using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a command that can be invoked by the user.
    /// </summary>
    public class HtmlElementInteractiveCommand : HtmlElement, IHtmlElementInteractive
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementInteractiveCommand()
            : base("command")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementInteractiveCommand(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementInteractiveCommand(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
