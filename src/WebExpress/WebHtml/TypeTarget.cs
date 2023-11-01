namespace WebExpress.WebHtml
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
        /// Umwandlung in einen Klartext
        /// </summary>
        /// <param name="target">Das Aufrufsziel</param>
        /// <returns>Der Klartext des Targets</returns>
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
