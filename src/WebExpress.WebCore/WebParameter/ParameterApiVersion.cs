namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a api version parameter with a key, value, and scope.
    /// </summary>
    public sealed class ParameterApiVersion : IParameterStatic
    {
        /// <summary>
        /// Gets the key that uniquely identifies the parameter in configuration or
        /// settings contexts.
        /// </summary>
        public static string Key => "_apiVersion";

        /// <summary>
        /// Gets or sets the scope of the parameter.
        /// </summary>
        public ParameterScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ParameterApiVersion()
        {
            Scope = ParameterScope.Url;
        }

        /// <summary>
        /// Initializes a new instance of the class with a specified value.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        public ParameterApiVersion(string value)
            : this()
        {
            Value = value;
        }

        /// <summary>
        /// Retrieves the unique key associated with the current instance.
        /// </summary>
        /// <returns>
        /// A string representing the unique key. This key is used for identifying 
        /// the instance in various operations.
        /// </returns>
        public string GetKey()
        {
            return Key;
        }
    }
}
