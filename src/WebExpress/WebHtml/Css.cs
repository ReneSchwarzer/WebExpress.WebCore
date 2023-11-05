using System.Linq;

namespace WebExpress.WebHtml
{
    public static class Css
    {
        /// <summary>
        /// Verbindet die angebenen CSS-Klassen zu einem String
        /// </summary>
        /// <param name="items">Die einzelnen CSS-Klassen</param>
        /// <returns>Die Css-Klassen als String</returns>
        public static string Concatenate(params string[] items)
        {
            return string.Join(' ', items.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct());
        }

        /// <summary>
        /// Entfernt die angegebenen CSS-Klassen aus dem Gesammtsring
        /// </summary>
        /// <param name="css">Die in einem gemeinsamen String verbundenen CSS-Klassen</param>
        /// <param name="remove">Die zu entfernenden CSS-Klassen</param>
        /// <returns>Die Css-Klassen als String</returns>
        public static string Remove(string css, params string[] remove)
        {
            return string.Join(' ', css.Split(' ').Where(x => !remove.Contains(x)));
        }
    }
}
