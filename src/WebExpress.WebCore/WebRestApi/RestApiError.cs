namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents an error returned by a REST API.
    /// </summary>
    public class RestApiError
    {
        /// <summary>
        /// Returns or sets the error code (e.g. "VALIDATION_FAILED").
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// Returns a human-readable message.
        /// </summary>
        public string Message { get; init; }

        /// <summary>
        /// Returns the name of the field or parameter affected by the operation.
        /// </summary>
        public string Field { get; init; }

        /// <summary>
        /// Represents an error returned by a REST API.
        /// </summary>
        /// <param name="message">The error message describing the issue. This parameter cannot be null or empty.</param>
        /// <param name="code">The optional error code associated with the error. This can be used to identify specific error types.</param>
        /// <param name="field">The optional name of the field that caused the error, if applicable.</param>
        public RestApiError(string message, string code = null, string field = null)
        {
            Message = message;
            Code = code;
            Field = field;
        }

        /// <summary>
        /// Returns a string representation of the object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Field)
                ? $"{Field}: {Message}"
                : Message;
        }
    }
}
