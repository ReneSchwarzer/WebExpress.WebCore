namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a parameter with a key, value, and scope.
    /// </summary>
    public interface IParameterStatic : IParameter
    {
        /// <summary>
        /// Gets the key of the parameter.
        /// </summary>
        static abstract string Key { get; }

        /// <summary>
        /// Retrieves the unique key associated with the current instance.
        /// </summary>
        /// <returns>
        /// A string representing the unique key. This key is used for identifying 
        /// the instance in various operations.
        /// </returns>
        string GetKey();
    }
}
