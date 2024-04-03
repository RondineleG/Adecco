public sealed class UnitOfWork(EntityFrameworkDataContext context) : IUnitOfWork
{
    private readonly EntityFrameworkDataContext _context = context;

    public async Task CompleteAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.BeginTransactionAsync();
        }
    }

    public async Task CommitTransactionAsync()
    {
        var transaction = _context.Database.CurrentTransaction;
        if (transaction != null)
        {
            await transaction.CommitAsync();
            transaction.Dispose();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        var transaction = _context.Database.CurrentTransaction;
        if (transaction != null)
        {
            await transaction.RollbackAsync();
            transaction.Dispose();
        }
    }
}
