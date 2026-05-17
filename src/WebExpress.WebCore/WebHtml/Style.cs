using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents utility methods for managing CSS styles.
    /// </summary>
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

        /// <summary>
        /// Replaces a style by first removing a style and then concatenating the new one.
        /// </summary>
        /// <param name="styles">The styles connected in a common string.</param>
        /// <param name="remove">The style to remove.</param>
        /// <param name="add">The style to add.</param>
        /// <returns>The styles as a string.</returns>
        public static string Replace(string styles, string remove, string add)
        {
            return Concatenate([.. Remove(styles, remove).Split(' '), add]);
        }
    }
}
