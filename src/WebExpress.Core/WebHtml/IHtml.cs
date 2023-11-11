using System.Text;

namespace WebExpress.Core.WebHtml
{
    public interface IHtml
    {
        /// <summary>
        /// Convert to a string using a StringBuilder.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="deep">The call depth.</param>
        void ToString(StringBuilder builder, int deep);
    }
}
