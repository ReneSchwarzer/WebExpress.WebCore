namespace WebExpress.Core.WebHtml
{
    public enum TypeTarget
    {
        None,
        Blank,
        Self,
        Parent,
        Top,
        Framename
    }

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
