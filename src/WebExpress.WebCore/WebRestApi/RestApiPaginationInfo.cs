using System;
using System.Text.Json.Serialization;

namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents pagination information for a REST API CRUD operation.
    /// </summary>
    public class RestApiPaginationInfo : IRestApiPaginationInfo
    {
        /// <summary>
        /// Gets or sets the current page number in a paginated result set.
        /// </summary>
        [JsonPropertyName("page")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of items to display per page in a paginated list.
        /// </summary>
        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total count of items.
        /// </summary>
        [JsonPropertyName("total")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets the total number of pages based on the total item count and the page size.
        /// </summary>
        [JsonPropertyName("totalPages")]
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
