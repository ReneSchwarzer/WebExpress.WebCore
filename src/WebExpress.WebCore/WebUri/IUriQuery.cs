namespace WebExpress.WebCore.WebUri
{
    /// <summary>
    /// The query part (e.g. ?title=Uniform_Resource_Identifier).
    /// </summary>
    public interface IUriQuery
    {
        /// <summary>
        /// Returns the key.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Returns the value.
        /// </summary>
        string Value { get; }
    }
}