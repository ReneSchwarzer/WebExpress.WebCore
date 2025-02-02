namespace WebExpress.WebCore.WebHtml
{
    /// <summary>
    /// Specifies the target for hyperlinks or forms.
    /// </summary>
    public enum TypeTarget
    {
        /// <summary>
        /// No target specified.
        /// </summary>
        None,

        /// <summary>
        /// Opens the link in a new window or tab.
        /// </summary>
        Blank,

        /// <summary>
        /// Opens the link in the same frame as it was clicked.
        /// </summary>
        Self,

        /// <summary>
        /// Opens the link in the parent frame.
        /// </summary>
        Parent,

        /// <summary>
        /// Opens the link in the full body of the window.
        /// </summary>
        Top,

        /// <summary>
        /// Opens the link in a named frame.
        /// </summary>
        Framename
    }

    /// <summary>
    /// Provides extension methods for the TypeTarget enum.
    /// </summary>
    public static class TypeTargetExtensions
    {
        /// <summary>
        /// Conversion into plain text.
        /// </summary>
        /// <param name="target">The call target.</param>
        /// <returns>The plain text of the target.</returns>
        public static string ToStringValue(this TypeTarget target)
        {
            return target switch
            {
                TypeTarget.Blank => "_blank",
                TypeTarget.Self => "_self",
                TypeTarget.Parent => "_parent",
                TypeTarget.Top => "_top",
                TypeTarget.Framename => "_framename",
                _ => string.Empty,
            };
        }
    }
}
