using Microsoft.AspNetCore.Mvc.Rendering;

namespace Logix.Application.Common
{
    /// <summary>
    /// Represents the result returned for a drop-down list (DDL) query that contains a list of items
    /// and an optional marker for pagination or incremental loading (LastSeenId).
    /// </summary>
    /// <typeparam name="TValue">The type of the value contained in each DDL item.</typeparam>
    public class DDLResult<TValue>
    {
        /// <summary>
        /// The list of items to render in a drop-down/select control.
        /// </summary>
        public List<DDLItem<TValue>> Items { get; set; } = new();

        /// <summary>
        /// Optional identifier of the last item seen by the client. Useful for cursor-based pagination or incremental loading.
        /// </summary>
        public TValue? LastSeenId { get; set; }
    }

    /// <summary>
    /// Represents a paged result for DDLs when using MVC's <see cref="SelectList"/> for server-side rendering.
    /// </summary>
    public class DDLDataPagedResult
    {
        /// <summary>
        /// MVC SelectList containing the items to render in the server-side select control.
        /// </summary>
        public SelectList Items { get; set; }

        /// <summary>
        /// Numeric marker representing the last seen id for pagination.
        /// </summary>
        public long LastSeenId { get; set; }
    }
}
