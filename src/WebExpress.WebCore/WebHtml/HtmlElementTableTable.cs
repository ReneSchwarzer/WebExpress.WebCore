using System.Collections.Generic;
using System.Text;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents a table, i.e. data with more than one dimension.
    /// </summary>
    public class HtmlElementTableTable : HtmlElement, IHtmlElementTable
    {
        /// <summary>
        /// Returns or sets the columns.
        /// </summary>
        public HtmlElementTableTr Columns { get; set; }

        /// <summary>
        /// Returns or sets the rows.
        /// </summary>
        public List<HtmlElementTableTr> Rows { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public HtmlElementTableTable()
            : base("table")
        {
            Columns = new HtmlElementTableTr();
            Rows = new List<HtmlElementTableTr>();
        }

        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            ToPreString(builder, deep);

            var column = new HtmlElementTableThead(Columns);
            column.ToString(builder, deep + 1);

            var body = new HtmlElementTableTbody(Rows);
            body.ToString(builder, deep + 1);

            ToPostString(builder, deep);
        }
    }
}
