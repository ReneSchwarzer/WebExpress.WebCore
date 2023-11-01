using System.Collections.Generic;
using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Markiert einFormular. Formulare bestehen typischerweise aus einer Reihe 
    /// von Kontrollelementen, deren Werte zur weiteren Verarbeitung an einen 
    /// Server übertragen werden.
    /// </summary>
    public class HtmlElementFormForm : HtmlElement, IHtmlElementForm
    {
        /// <summary>
        /// Returns or sets the name of the form.
        /// </summary>
        public string Name
        {
            get => GetAttribute("name");
            set => SetAttribute("name", value);
        }

        /// <summary>
        /// Liefert oder setzt die Zeichenkodierung
        /// </summary>
        public string AcceptCharset
        {
            get => GetAttribute("accept-charset");
            set => SetAttribute("accept-charset", value);
        }

        /// <summary>
        /// Liefert oder setzt die Zeichenkodierung
        /// </summary>
        public TypeEnctype Enctype
        {
            get => TypeEnctypeExtensions.Convert(GetAttribute("enctype"));
            set => SetAttribute("enctype", value.Convert());
        }
        /// <summary>
        /// Liefert oder setzt den Methode Post oder get
        /// </summary>
        public string Method
        {
            get => GetAttribute("method");
            set => SetAttribute("method", value);
        }

        /// <summary>
        /// Liefert oder setzt die URL 
        /// </summary>
        public string Action
        {
            get => GetAttribute("action");
            set => SetAttribute("action", value);
        }

        /// <summary>
        /// Liefert oder setzt das Zielfenster
        /// </summary>
        public string Target
        {
            get => GetAttribute("target");
            set => SetAttribute("target", value);
        }

        /// <summary>
        /// Returns the elements.
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementFormForm()
            : base("form")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementFormForm(string text)
            : this()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nodes">The content of the html element.</param>
        public HtmlElementFormForm(params IHtmlNode[] nodes)
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
