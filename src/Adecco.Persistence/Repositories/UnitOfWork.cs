namespace Adecco.Persistence.Repositories;

public sealed class UnitOfWork(ApplicattionDataContext context) : IUnitOfWork
{
    private readonly ApplicattionDataContext _context = context;
    private IDbContextTransaction _currentTransaction;

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
