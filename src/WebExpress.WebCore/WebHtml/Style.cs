using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    public static class Style
    {
        /// <summary>
        /// Connects the specifying styles into a string.
        /// </summary>
        /// <param name="items">The individual styles.</param>
        /// <returns>The styles as a string.</returns>
        public static string Concatenate(params string[] items)
        {
            return string.Join(" ", items.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Removes the specified styles from the string.
        /// </summary>
        /// <param name="styles">The styles connected in a common string.</param>
        /// <param name="remove">The styles to remove.</param>
        /// <returns>The styles as a string.</returns>
        public static string Remove(string styles, params string[] remove)
        {
            return string.Join(" ", styles.Split(' ').Where(x => !remove.Contains(x)));
        }
    }
}
