namespace ZawatSys.MicroLib.AI.Application.Abstractions.Repositories;

/// <summary>
/// Unit of Work interface for coordinating multiple repository operations in a single transaction.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Commits all changes to the database asynchronously.
    /// </summary>
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits all changes to the database synchronously.
    /// </summary>
    int Commit();

    /// <summary>
    /// Rolls back all changes.
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an operation within a transaction with retry logic.
    /// </summary>
    Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default);
}
