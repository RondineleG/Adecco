namespace Adecco.Persistence.Repositories;

public sealed class ClenteRepository(ApplicattionDataContext context)
    : BaseRepository(context), IClienteRepository
{
    public async Task<IEnumerable<Cliente>> ListAsync(string? nome, string? email, string? cpf)
    {
        IQueryable<Cliente> query = _context
            .Clientes.Include(p => p.Contatos)
            .Include(p => p.Enderecos);
        if (!string.IsNullOrEmpty(nome))
            query = query.Where(p => p.Nome.Contains(nome));
        if (!string.IsNullOrEmpty(email))
            query = query.Where(p => p.Email == email);
        if (!string.IsNullOrEmpty(cpf))
            query = query.Where(p => p.CPF == cpf);
        return await query.ToListAsync();
    }

    public async Task<Cliente?> FindByIdAsync(int id)
    {
        var cliente = await _context
            .Clientes.Include(p => p.Contatos)
            .Include(p => p.Enderecos)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (cliente == null)
            return null;
        return cliente;
    }

    public async Task AddAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }

    public void Update(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
    }

    public void Remove(Cliente cliente)
    {
        _context.Clientes.Remove(cliente);
    }
}
