using System.Linq;
using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Steht für eine Messskala (oder deren Teilwerte) innerhalb eines bekannten Bereichs.
    /// </summary>
    public class HtmlElementFormMeter : HtmlElement, IHtmlElementForm
    {
        /// <summary>
        /// Returns or sets the text.
        /// </summary>
        public string Text
        {
            get => string.Join(string.Empty, Elements.Where(x => x is HtmlText).Select(x => (x as HtmlText).Value));
            set { Elements.Clear(); Elements.Add(new HtmlText(value)); }
        }

        /// <summary>
        /// Returns or sets the value.
        /// </summary>
        public string Value
        {
            get => GetAttribute("value");
            set => SetAttribute("value", value);
        }

        /// <summary>
        /// Liefert oder setzt die untere Grenze der Skala
        /// </summary>
        public string Min
        {
            get => GetAttribute("min");
            set => SetAttribute("min", value);
        }

        /// <summary>
        /// Liefert oder setzt die obere Grenze der Skala
        /// </summary>
        public string Max
        {
            get => GetAttribute("max");
            set => SetAttribute("max", value);
        }

        /// <summary>
        /// Liefert oder setzt die obere Grenze des "Niedrig"-Bereichs der Skala
        /// </summary>
        public string Low
        {
            get => GetAttribute("low");
            set => SetAttribute("low", value);
        }

        /// <summary>
        /// Liefert oder setzt die untere Grenze des "Hoch"-Bereichs der Skala
        /// </summary>
        public string High
        {
            get => GetAttribute("high");
            set => SetAttribute("high", value);
        }

        /// <summary>
        /// Liefert oder setzt den optimaler Wert der Skala
        /// </summary>
        public string Optimum
        {
            get => GetAttribute("optimum");
            set => SetAttribute("optimum", value);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementFormMeter()
            : base("meter")
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">The content of the html element.</param>
        public HtmlElementFormMeter(string text)
            : this()
        {
            Text = text;
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
