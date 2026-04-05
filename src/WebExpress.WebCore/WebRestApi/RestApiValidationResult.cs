using System.Collections.Generic;
using System.Linq;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents the result of a REST API validation, including any validation errors encountered.
    /// </summary>
    /// <remarks>
    /// This class provides a way to collect and inspect validation errors that occur 
    /// during the processing of a REST API request. It includes methods to add individual 
    /// or multiple errors, and properties to check the overall validity of the result.
    /// </remarks>
    public class RestApiValidationResult : IRestApiValidationResult
    {
        private readonly List<RestApiError> _errors = [];

        /// <summary>
        /// Returns a read-only collection of errors encountered during the API operation.
        /// </summary>
        public IEnumerable<RestApiError> Errors => _errors.AsReadOnly();

        /// <summary>
        /// Returns a value indicating whether the current state is valid.
        /// </summary>
        /// <remarks>The state is considered valid if there are no errors present.</remarks>
        public bool IsValid => _errors.Count == 0;

        /// <summary>
        /// Adds a new error to the collection with the specified message, field, and code.
        /// </summary>
        /// <remarks>
        /// Use this method to record errors encountered during an operation, 
        /// optionally associating them with a specific field or error code.
        /// </remarks>
        /// <param name="message">
        /// The error message describing the issue. This parameter is required and 
        /// cannot be null or empty.
        /// </param>
        /// <param name="field">
        /// The name of the field associated with the error, if applicable. This 
        /// parameter is optional and can be null.
        /// </param>
        /// <param name="code">
        /// A code representing the type or category of the error, if applicable. 
        /// This parameter is optional and can be null.
        /// </param>
        /// <returns>The current instance for method chaining.</returns>
        public IRestApiValidationResult Add(string message, string field = null, string code = null)
        {
            _errors.Add(new RestApiError(message, code, field));

            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="RestApiError"/> instances to the collection.
        /// </summary>
        /// <remarks>
        /// This method appends the specified errors to the existing collection. If 
        /// the array is empty, no changes are made.
        /// </remarks>
        /// <param name="errors">An array of error objects to add.</param>
        /// <returns>The current instance for method chaining.</returns>
        public IRestApiValidationResult Add(params RestApiError[] errors)
        {
            _errors.AddRange(errors);

            return this;
        }

        /// <summary>
        /// Adds one or more <see cref="RestApiError"/> instances to the collection.
        /// </summary>
        /// <remarks>
        /// This method appends the specified errors to the existing collection. If 
        /// the array is empty, no changes are made.
        /// </remarks>
        /// <param name="errors">An array of error objects to add.</param>
        /// <returns>The current instance for method chaining.</returns>
        public IRestApiValidationResult AddRange(IEnumerable<RestApiError> errors)
        {
            if (errors is not null)
            {
                _errors.AddRange(errors);
            }

            return this;
        }

        /// <summary>
        /// Returns a string representation of the current object, summarizing all errors.
        /// </summary>
        /// <returns>A semicolon-separated string of error descriptions.</returns>
        public override string ToString()
        {
            return string.Join("; ", _errors.Select(e => e.ToString()));
        }

        /// <summary>
        /// Converts the collection of errors to a JSON-formatted string.
        /// </summary>
        /// <remarks>
        /// Each error in the collection is serialized as an object containing the 
        /// properties <c>Code</c>, <c>Message</c>, and <c>Field</c>. The resulting 
        /// JSON string represents  an array of these objects.
        /// </remarks>
        /// <returns>
        /// A JSON-formatted string representing the collection of errors. If 
        /// the collection is empty, the method returns an empty JSON array 
        /// (<c>[]</c>).
        /// </returns>
        public virtual string ToJson()
        {
            return System.Text.Json.JsonSerializer.Serialize(_errors.Select(e => new
            {
                code = e.Code,
                message = e.Message,
                field = e.Field
            }));
        }
    }
}
