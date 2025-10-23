using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logix.Application.Common
{
    /// <summary>
    /// Abstraction for a unit-of-work which exposes the application's <see cref="DbContext"/>
    /// and manages transactions and the save lifecycle.
    /// Implementations should encapsulate transaction boundaries and call <see cref="CompleteAsync"/> to persist changes.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Returns the underlying <see cref="DbContext"/> instance used by this unit of work.
        /// </summary>
        /// <returns>The active <see cref="DbContext"/>.</returns>
        DbContext GetContext();

        /// <summary>
        /// Begins a new database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <remarks>
        /// Implementations typically call <c>DbContext.Database.BeginTransactionAsync</c> and store the transaction
        /// so that subsequent <see cref="CommitTransactionAsync"/> or <see cref="RollbackTransactionAsync"/> can act on it.
        /// </remarks>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <remarks>
        /// Implementations should flush pending changes (if not already flushed) and commit the transaction.
        /// </remarks>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Persists all changes to the database and returns the number of affected rows.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the save operation.</param>
        /// <returns>The number of state entries written to the database.</returns>
        /// <remarks>
        /// Typical implementations delegate to <c>DbContext.SaveChangesAsync</c>.
        /// When using transactions, call this inside the transaction boundary before committing.
        /// </remarks>
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <remarks>
        /// Implementations should ensure the transaction (if present) is rolled back and any resources disposed.
        /// </remarks>
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    }
}
