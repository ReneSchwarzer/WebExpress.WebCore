using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Provides utility methods for working with CSS classes.
    /// </summary>
    public static class Css
    {
        /// <summary>
        /// Joins the specified CSS classes into a single string, ensuring no duplicates 
        /// and ignoring null or whitespace entries.
        /// </summary>
        /// <param name="items">The individual CSS classes to join.</param>
        /// <returns>A string containing the concatenated CSS classes.</returns>
        public static string Concatenate(params string[] items)
        {
            return string.Join(' ', items.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Joins the specified CSS classes into a single string, starting with a required first 
        /// class, ensuring no duplicates and ignoring null or whitespace entries.
        /// </summary>
        /// <param name="first">The first CSS class, which is required.</param>
        /// <param name="items">Additional CSS classes to join.</param>
        /// <returns>A string containing the concatenated CSS classes.</returns>
        public static string Concatenate(string first, params string[] items)
        {
            return string.Join(' ', new[] { first }.Union(items).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Joins the specified CSS classes into a single string, starting with a required first 
        /// class, ensuring no duplicates and ignoring null or whitespace entries.
        /// </summary>
        /// <param name="first">The first CSS class, which is required.</param>
        /// <param name="items">Additional CSS classes to join.</param>
        /// <returns>A string containing the concatenated CSS classes.</returns>
        public static string Concatenate(string first, IEnumerable<string> items)
        {
            return string.Join(' ', new[] { first }.Union(items).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Removes the specified CSS classes from a string of concatenated CSS classes.
        /// </summary>
        /// <param name="css">The string containing concatenated CSS classes.</param>
        /// <param name="remove">The CSS classes to remove from the string.</param>
        /// <returns>A string containing the remaining CSS classes after removal.</returns>
        public static string Remove(string css, params string[] remove)
        {
            return string.Join(' ', css.Split(' ').Where(x => !remove.Contains(x)));
        }

        /// <summary>
        /// Replaces a CSS class by first removing a class and then concatenating the new one.
        /// </summary>
        /// <param name="css">The string containing concatenated CSS classes.</param>
        /// <param name="remove">The CSS class to remove from the string.</param>
        /// <param name="add">The CSS class to add to the string.</param>
        /// <returns>A string containing the updated CSS classes.</returns>
        public static string Replace(string css, string remove, string add)
        {
            return Concatenate([.. Remove(css, remove).Split(' '), add]);
        }
    }
}
