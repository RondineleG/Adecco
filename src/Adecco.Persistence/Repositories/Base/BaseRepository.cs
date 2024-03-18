namespace Adecco.Persistence.Repositories.Base;

public abstract class BaseRepository
{
    public BaseRepository(ApplicattionDataContext context) => _context = context;

    protected readonly ApplicattionDataContext _context;
}