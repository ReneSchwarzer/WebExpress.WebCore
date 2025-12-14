using System.Collections.Generic;

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
    public interface IRestApiValidationResult
    {
        /// <summary>
        /// Returns a read-only collection of errors encountered during the API operation.
        /// </summary>
        IEnumerable<RestApiError> Errors { get; }

        /// <summary>
        /// Returns a value indicating whether the current state is valid.
        /// </summary>
        /// <remarks>The state is considered valid if there are no errors present.</remarks>
        bool IsValid { get; }

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
        IRestApiValidationResult Add(string message, string field = null, string code = null);

        /// <summary>
        /// Adds one or more <see cref="RestApiError"/> instances to the collection.
        /// </summary>
        /// <remarks>
        /// This method appends the specified errors to the existing collection. If 
        /// the array is empty, no changes are made.
        /// </remarks>
        /// <param name="errors">An array of error objects to add.</param>
        /// <returns>The current instance for method chaining.</returns>
        IRestApiValidationResult Add(params RestApiError[] errors);

        /// <summary>
        /// Adds one or more <see cref="RestApiError"/> instances to the collection.
        /// </summary>
        /// <remarks>
        /// This method appends the specified errors to the existing collection. If 
        /// the array is empty, no changes are made.
        /// </remarks>
        /// <param name="errors">An array of error objects to add.</param>
        /// <returns>The current instance for method chaining.</returns>
        IRestApiValidationResult AddRange(IEnumerable<RestApiError> errors);

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
        string ToJson();
    }
}
