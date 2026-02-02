namespace WebExpress.WebCore.WebAttribute
{
    /// <summary>
    /// Represents an attribute that assigns a title to a page, setting page, or status page.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TitleAttribute : System.Attribute, IPageAttribute, ISettingPageAttribute, IStatusPageAttribute
    {
        /// <summary>
        /// Returns the title associated with the current instance.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="display">The display text.</param>
        public TitleAttribute(string display)
        {
            Title = display;
        }
    }
}
