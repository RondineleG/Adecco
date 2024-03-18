namespace Adecco.Persistence.Repositories;

public class EnderecoRepository : BaseRepository, IEnderecoRepository
{
    public EnderecoRepository(ApplicattionDataContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Endereco>> ListAsync()
    { return await _context.Enderecos.ToListAsync(); }

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