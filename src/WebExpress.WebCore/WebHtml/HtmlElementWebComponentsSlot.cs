﻿using System.Collections.Generic;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a placeholder.
    /// </summary>
    public class HtmlElementWebFragmentsSlot : HtmlElement, IHtmlElementWebFragments
    {
        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public HtmlElementWebFragmentsSlot()
            : base("slot")
        {
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementWebFragmentsSlot(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementWebFragmentsSlot(IEnumerable<IHtmlNode> nodes)
            : this()
        {
            base.Elements.AddRange(nodes);
        }
    }
}
