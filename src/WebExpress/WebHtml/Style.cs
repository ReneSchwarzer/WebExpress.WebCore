using System.Linq;

namespace WebExpress.WebHtml
{
    public static class Style
    {
        /// <summary>
        /// Verbindet die angebenen Styles zu einem String
        /// </summary>
        /// <param name="items">Die einzelnen Styles</param>
        /// <returns>Die Styles als String</returns>
        public static string Concatenate(params string[] items)
        {
            return string.Join(" ", items.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Entfernt die angegebenen Styles aus dem Gesammtsring
        /// </summary>
        /// <param name="styles">Die in einem gemeinsamen String verbundenen Styles</param>
        /// <param name="remove">Die zu entfernenden Styles</param>
        /// <returns>Die Styles als String</returns>
        public static string Remove(string styles, params string[] remove)
        {
            return string.Join(" ", styles.Split(' ').Where(x => !remove.Contains(x)));
        }
    }
}
