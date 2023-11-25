using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    public static class Css
    {
        /// <summary>
        /// Joins the specifying css classes into a string.
        /// </summary>
        /// <param name="items">The individual css classes.</param>
        /// <returns>The css classes as a string.</returns>
        public static string Concatenate(params string[] items)
        {
            return string.Join(' ', items.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Removes the specified css classes from the string.
        /// </summary>
        /// <param name="css">The css classes connected in a common string.</param>
        /// <param name="remove">The css classes to remove.</param>
        /// <returns>The css classes as a string.</returns>
        public static string Remove(string css, params string[] remove)
        {
            return string.Join(' ', css.Split(' ').Where(x => !remove.Contains(x)));
        }
    }
}
