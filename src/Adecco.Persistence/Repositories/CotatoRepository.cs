namespace Adecco.Persistence.Repositories;

public class CotatoRepository : BaseRepository, IContatoRepository
{
    public CotatoRepository(ApplicattionDataContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Contato>> ListAsync()
    {
        return await _context.Contatos.ToListAsync();
    }

    public async Task AddAsync(Contato contato)
    {
        await _context.Contatos.AddAsync(contato);
    }

    public async Task<Contato> FindByIdAsync(int? id)
    {
        return await _context.Contatos.FindAsync(id);
    }

    public void Update(Contato contato)
    {
        _context.Contatos.Update(contato);
    }

    public void Remove(Contato contato)
    {
        _context.Contatos.Remove(contato);
    }
}