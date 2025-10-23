using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logix.Application.Common
{
    /// <summary>
    /// Represents an item used in drop-down lists (DDL) or select controls.
    /// The generic parameter <typeparamref name="TValue"/> allows the value to be any type (int, long, string, GUID, etc.).
    /// </summary>
    /// <typeparam name="TValue">Type of the Value carried by the list item.</typeparam>
    public class DDLItem<TValue>
    {
        /// <summary>
        /// The display text shown to the user in the drop-down.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The underlying value associated with this item. This is the value submitted by forms or used for binding.
        /// </summary>
        public TValue Value { get; set; } = default!;
    }
}
