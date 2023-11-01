using System;
using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Extension methods for HTMLEelements.
    /// </summary>
    public static class HTMLElementExtension
    {
        /// <summary>
        /// Adds a css class.
        /// </summary>
        /// <param name="html">The HTML element to extend.</param>
        /// <param name="cssClass">The class to add.</param>
        /// <returns>The HTML element extended by the checkout.</returns>
        public static IHtmlNode AddClass(this IHtmlNode html, string cssClass)
        {
            if (html is HtmlElement)
            {
                var element = html as HtmlElement;

                var list = new List<string>(element.Class.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).Select(x => x.ToLower()).ToList();

                if (!list.Contains(cssClass.ToLower()))
                {
                    list.Add(cssClass.ToLower());
                }

                var css = string.Join(' ', list);

                element.Class = css;
            }

            return html;
        }

        /// <summary>
        /// Removes a css class.
        /// </summary>
        /// <param name="html">The HTML element.</param>
        /// <param name="cssClass">The class to remove.</param>
        /// <returns>The HTML element reduced by the checkout.</returns>
        public static IHtmlNode RemoveClass(this IHtmlNode html, string cssClass)
        {
            if (cssClass == null) return html;

            if (html is HtmlElement)
            {
                var element = html as HtmlElement;

                var list = new List<string>(element.Class.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).Select(x => x.ToLower()).ToList();

                if (list.Contains(cssClass.ToLower()))
                {
                    list.Remove(cssClass.ToLower());
                }

                var css = string.Join(' ', list);

                element.Class = css;
            }

            return html;
        }

        /// <summary>
        /// Adds a style.
        /// </summary>
        /// <param name="html">The HTML element to extend.</param>
        /// <param name="cssStyle">The class to add.</param>
        /// <returns>The HTML element extended by the checkout.</returns>
        public static IHtmlNode AddStyle(this IHtmlNode html, string cssStyle)
        {
            if (html is HtmlElement)
            {
                var element = html as HtmlElement;

                var list = new List<string>(element.Style.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).Select(x => x.ToLower()).ToList();

                if (!list.Contains(cssStyle.ToLower()))
                {
                    list.Add(cssStyle.ToLower());
                }

                var css = string.Join(' ', list);

                element.Style = css;
            }

            return html;
        }

        /// <summary>
        /// Removes a style.
        /// </summary>
        /// <param name="html">The HTML element.</param>
        /// <param name="cssStyle">Der Style, welcher entfernt werden soll</param>
        /// <returns>The HTML element reduced by the checkout.</returns>
        public static IHtmlNode RemoveStyle(this IHtmlNode html, string cssStyle)
        {
            if (html is HtmlElement)
            {
                var element = html as HtmlElement;

                var list = new List<string>(element.Style.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)).Select(x => x.ToLower()).ToList();

                if (list.Contains(cssStyle?.ToLower()))
                {
                    list.Remove(cssStyle.ToLower());
                }

                var css = string.Join(' ', list);

                element.Style = css;
            }

            return html;
        }
    }
}
