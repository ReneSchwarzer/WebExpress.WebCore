namespace WebExpress.WebUri
{
    /// <summary>
    /// The query part (e.g. ?title=Uniform_Resource_Identifier&action=submit).
    /// </summary>
    public class UriQuerry
    {
        /// <summary>
        /// Returns the key.
        /// </summary>
        public string Key { get; protected set; }

        /// <summary>
        /// Returns the value.
        /// </summary>
        public string Value { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public UriQuerry(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}