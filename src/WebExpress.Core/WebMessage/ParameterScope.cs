namespace WebExpress.Core.WebMessage
{
    /// <summary>
    /// Defines the scopes of the parameter.
    /// </summary>
    public enum ParameterScope
    {
        /// <summary>
        /// No classification.
        /// </summary>
        None,

        /// <summary>
        /// Parameter refers to a part of the uri.
        /// </summary>
        Url,

        /// <summary>
        /// Parameter refers to the session.
        /// </summary>
        Session,

        /// <summary>
        /// Parameter refers to url parameters (GET or POST).
        /// </summary>
        Parameter
    }
}
