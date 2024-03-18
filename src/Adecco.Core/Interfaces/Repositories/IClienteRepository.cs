namespace Adecco.Core.Interfaces.Repositories;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>> ListAsync(string? nome, string? email, string? cpf);

    Task AddAsync(Cliente cliente);

    Task<Cliente> FindByIdAsync(int id);

    void Update(Cliente cliente);

    void Remove(Cliente cliente);
}