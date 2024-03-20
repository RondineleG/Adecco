namespace Adecco.Persistence.Repositories.Base;
public abstract class BaseRepository(ApplicattionDataContext context)
{
    protected readonly ApplicattionDataContext _context = context;
}