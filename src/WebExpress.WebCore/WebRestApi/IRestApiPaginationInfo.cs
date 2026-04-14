namespace WebExpress.WebCore.WebRestApi
{
    /// <summary>
    /// Represents pagination information for a REST API CRUD operation.
    /// </summary>
    public interface IRestApiPaginationInfo
    {
        /// <summary>
        /// Gets the current page number in a paginated result set.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets the number of items to display per page in a paginated list.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets the total count of items.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Gets the total number of pages based on the total item count and the page size.
        /// </summary>
        int TotalPages { get; }
    }
}
