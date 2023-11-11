using System.Linq;
using System.Text;

namespace WebExpress.Core.WebHtml
{
    /// <summary>
    /// Represents a scale (or its sub-values) within a known range.
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
        /// Returns or sets the lower limit of the scale.
        /// </summary>
        public string Min
        {
            get => GetAttribute("min");
            set => SetAttribute("min", value);
        }

        /// <summary>
        /// Returns or sets the upper limit of the scale.
        /// </summary>
        public string Max
        {
            get => GetAttribute("max");
            set => SetAttribute("max", value);
        }

        /// <summary>
        /// Returns or sets the upper limit of the "low" range of the scale.
        /// </summary>
        public string Low
        {
            get => GetAttribute("low");
            set => SetAttribute("low", value);
        }

        /// <summary>
        /// Returns or sets the lower bound of the "high" range of the scale.
        /// </summary>
        public string High
        {
            get => GetAttribute("high");
            set => SetAttribute("high", value);
        }

        /// <summary>
        /// Returns or sets the optimal value of the scale.
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
