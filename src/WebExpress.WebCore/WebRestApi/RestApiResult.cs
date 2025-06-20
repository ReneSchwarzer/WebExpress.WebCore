using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebExpress.WebCore.WebMessage;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents the result of a CRUD operation performed via a REST API.
    /// </summary>
    public class RestApiResult : IRestApiResult
    {
        private readonly List<RestApiError> _errors = [];
        private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

        /// <summary>
        /// Returns a value indicating whether the operation was successful.
        /// </summary>
        [JsonIgnore()]
        public bool Success => !Errors.Any();

        /// <summary>
        /// Returns the collection of error messages.
        /// </summary>
        [JsonPropertyName("errors")]
        public IEnumerable<RestApiError> Errors => _errors;

        /// <summary>
        /// Returns or sets the data associated with the index item.
        /// </summary>
        [JsonPropertyName("data")]
        public object Data { get; set; }

        /// <summary>
        /// Returns or sets the pagination information for the current API request.
        /// </summary>
        [JsonPropertyName("pagination")]
        public RestApiPaginationInfo Pagination { get; set; }

        /// <summary>
        /// Adds one or more error messages to the result.
        /// </summary>
        /// <param name="errors">An array of error messages to add. Each message should describe an issue encountered during the operation.</param>
        /// <returns>The current instance, allowing for method chaining.</returns>
        public RestApiResult AddError(params RestApiError[] errors)
        {
            _errors.AddRange(errors);

            return this;
        }

        /// <summary>
        /// Adds one or more error messages to the result.
        /// </summary>
        /// <param name="errors">An array of error messages to add. Each message should describe an issue encountered during the operation.</param>
        /// <returns>The current instance, allowing for method chaining.</returns>
        public RestApiResult AddError(IEnumerable<RestApiError> errors)
        {
            _errors.AddRange(errors);

            return this;
        }

        /// <summary>
        /// Converts the current instance into a <see cref="Response"/> object.
        /// </summary>
        /// <returns>A Response object representing the result of the conversion.</returns>
        public virtual Response ToResponse()
        {
            if (Data != null)
            {
                var jsonData = JsonSerializer.Serialize(Data, _jsonOptions);
                var content = Encoding.UTF8.GetBytes(jsonData);

                return new ResponseOK
                {
                    Content = content
                }
                .AddHeaderContentType("application/json");
            }

            return new ResponseBadRequest
            {
                Content = Encoding.UTF8.GetBytes("No data provided.")
            };
        }

        /// <summary>
        /// Creates a successful result for a REST API operation, containing the specified 
        /// data and optional pagination information.
        /// </summary>
        /// <param name="data">The data item to include in the result. Cannot be null.</param>
        /// <param name="pagination">Optional pagination information to include in the result. If null, no pagination details are provided.</param>
        /// <returns>Containing the specified data and pagination information.</returns>
        public static IRestApiResult Ok(object data, RestApiPaginationInfo pagination = null)
        {
            return new RestApiResult
            {
                Data = data,
                Pagination = pagination
            };
        }

        /// <summary>
        /// Creates a failed result with the specified error messages.
        /// </summary>
        /// <param name="errors">An array of error messages describing the failure. Cannot be null, but may be empty.</param>
        /// <returns>Containing the provided error messages.</returns>
        public static IRestApiResult Fail(params RestApiError[] errors)
        {
            return new RestApiResult()
                .AddError(errors);
        }
    }
}