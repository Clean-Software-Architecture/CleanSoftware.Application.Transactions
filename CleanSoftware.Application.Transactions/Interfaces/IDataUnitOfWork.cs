using System.Data;

namespace CleanSoftware.Application.Transactions.Interfaces
{
    public interface IDataUnitOfWork
    {
        bool HasActiveTransaction { get; }

        Task BeginTransactionAsync(
            IsolationLevel isolationLevel,
            CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(
            DateTime commitTime,
            CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync();
    }
}