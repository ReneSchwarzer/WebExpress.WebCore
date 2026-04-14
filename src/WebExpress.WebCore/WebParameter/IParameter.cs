namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a parameter with a key, value, and scope.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Gets or sets the scope of the parameter.
        /// </summary>
        ParameterScope Scope { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        string Value { get; set; }
    }
}
