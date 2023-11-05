using System.Text;

namespace WebExpress.WebHtml
{
    /// <summary>
    /// Ein Attribut ohne wert
    /// z.B. required in input <input required>
    /// </summary>
    public class HtmlAttributeNoneValue : IHtmlAttribute
    {
        /// <summary>
        /// Returns or sets the name. des Attributes
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlAttributeNoneValue()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Der Name</param>
        public HtmlAttributeNoneValue(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public virtual void ToString(StringBuilder builder, int deep)
        {
            builder.Append(Name);
        }
    }
}
