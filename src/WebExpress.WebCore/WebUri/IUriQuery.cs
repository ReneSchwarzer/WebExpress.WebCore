namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// The query part (e.g. ?title=Uniform_Resource_Identifier).
    /// </summary>
    public interface IUriQuery
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        string Value { get; }
    }
}