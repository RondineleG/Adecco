namespace Adecco.Persistence.Repositories;

public sealed class ClenteRepository(EntityFrameworkDataContext context)
    : BaseRepository(context),
        IClienteRepository
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

    public async Task<Cliente> FindByIdAsync(int id)
    {
        var cliente = await _context
            .Clientes.Include(p => p.Contatos)
            .Include(p => p.Enderecos)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (cliente == null)
        {
            throw new Exception($"Cliente com o ID {id} não encontrado.");
        }
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

    public async Task<CustomResult<Cliente>> GetAsync(int id, CancellationToken cancellationToken)
    {
        var cliente = await _context.Clientes.FindAsync( id , cancellationToken);
        if (cliente != null)
        {
            return CustomResult<Cliente>.Success(cliente);
        }
        else
        {
            return CustomResult<Cliente>.EntityNotFound("Cliente", id, "Cliente não encontrado.");
        }
    }


}
