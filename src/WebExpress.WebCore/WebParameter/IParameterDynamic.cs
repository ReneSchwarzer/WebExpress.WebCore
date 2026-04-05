namespace WebExpress.WebCore.WebParameter
{
    /// <summary>
    /// Represents a parameter with a key, value, and scope.
    /// </summary>
    public interface IParameterDynamic : IParameter
    {
        /// <summary>
        /// Returns the key of the parameter.
        /// </summary>
        string Key { get; }
    }
}
