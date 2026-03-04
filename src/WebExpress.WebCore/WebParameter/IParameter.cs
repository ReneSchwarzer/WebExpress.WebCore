namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a parameter with a key, value, and scope.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Returns or sets the scope of the parameter.
        /// </summary>
        ParameterScope Scope { get; set; }

        /// <summary>
        /// Returns the value of the parameter.
        /// </summary>
        string Value { get; set; }
    }
}
