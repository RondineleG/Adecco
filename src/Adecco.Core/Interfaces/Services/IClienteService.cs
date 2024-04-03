namespace Adecco.Core.Interfaces.Services;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> ListAsync(string? nome, string? email, string? cpf);

    Task<Cliente> FindByIdAsync(int id);

    Task<CustomResult<Cliente>> GetAsync(int id, CancellationToken cancellationToken);

    Task<ClienteResponse> SaveAsync(Cliente cliente);

    Task<ClienteResponse> UpdateAsync(int id, Cliente cliente);

    Task<ClienteResponse> DeleteAsync(int id);
}
