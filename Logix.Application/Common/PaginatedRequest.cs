namespace Logix.Application.Common
{

    /// <summary>
    /// Marker interface for paginated requests that carry a filter payload and paging parameters.
    /// </summary>
    /// <typeparam name="T">Type of the filter object that contains query criteria.</typeparam>
    public interface IPaginatedRequest<T>
    {
        /// <summary>
        /// Filter object containing criteria used to restrict the query results.
        /// </summary>
        T Filter { get; }

        /// <summary>
        /// 1-based page number. Handlers should treat values less than 1 as page 1.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Number of items per page. Handlers may apply limits or defaults if the value is out-of-range.
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Default implementation of <see cref="IPaginatedRequest{T}"/> providing common paging fields and a filter.
    /// </summary>
    /// <typeparam name="T">Type of the filter payload.</typeparam>
    public class PaginatedRequest<T> : IPaginatedRequest<T>
    {
        /// <summary>
        /// Filter object containing the search or filter criteria for the request.
        /// </summary>
        public T Filter { get; set; }

        /// <summary>
        /// 1-based page number. Defaults to 1.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of items per page. Defaults to 10.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Optional cursor-style identifier for pagination (useful for keyset pagination).
        /// Handlers can use this <see cref="LastSeenId"/> to return the next page after the given id.
        /// </summary>
        public long LastSeenId { get; set; } = 0;  // Use long for LastSeenId to accommodate large identifier spaces
    }


}

