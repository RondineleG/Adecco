using Adecco.Core.Entities;

namespace Adecco.Persistence.Repositories;

public class EnderecoRepository : BaseRepository, IEnderecoRepository
{
    public EnderecoRepository(ApplicattionDataContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Endereco>> ListAsync(int? clienteId, int? enderecoId)
    {

        IQueryable<Endereco> query = _context.Enderecos;
        if (clienteId is not null) query = query.Where(c => c.ClienteId == clienteId);
        if (enderecoId is not null) query = query.Where(c => c.Id == enderecoId);
        return await query.ToListAsync();
    }

    public async Task AddAsync(Endereco endereco)
    {
        await _context.Enderecos.AddAsync(endereco);
    }

    public async Task<Endereco> FindByIdAsync(int? id)
    {
        return await _context.Enderecos.FindAsync(id);
    }

    public void Update(Endereco endereco)
    {
        _context.Enderecos.Update(endereco);
    }

    public void Remove(Endereco endereco)
    {
        _context.Enderecos.Remove(endereco);
    }
}