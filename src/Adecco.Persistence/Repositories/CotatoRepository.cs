namespace Adecco.Persistence.Repositories;

public sealed class CotatoRepository(ApplicattionDataContext context)
    : BaseRepository(context),
        IContatoRepository
{
    public async Task<IEnumerable<Contato>> ListAsync(int? clienteId, int? contatoId)
    {
        IQueryable<Contato> query = _context.Contatos;
        if (clienteId is not null)
            query = query.Where(c => c.ClienteId == clienteId);
        if (contatoId is not null)
            query = query.Where(c => c.Id == contatoId);
        return await query.ToListAsync();
    }

    public async Task AddAsync(Contato contato)
    {
        await _context.Contatos.AddAsync(contato);
    }

    public async Task<Contato> FindByIdAsync(int? id)
    {
        var contato = await _context.Contatos.FindAsync(id);
        if (contato == null)
        {
            throw new KeyNotFoundException("Contato n�o encontrado.");
        }
        return contato;
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
