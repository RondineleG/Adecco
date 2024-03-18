namespace Adecco.Core.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}