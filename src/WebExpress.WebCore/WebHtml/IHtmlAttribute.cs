namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Represents an HTML attribute that can be added to an HTML element.
    /// </summary>
    public interface IHtmlAttribute : IHtml
    {
        /// <summary>
        /// Gets or sets the attribute name.
        /// </summary>
        string Name { get; set; }

    }
}
