namespace Adecco.Core.Interfaces.Services;

public interface IEnderecoService
{
    Task<IEnumerable<Endereco>> ListAsync(int? clienteId, int? enderecoId);

    Task<Endereco> FindByIdAsync(int id);

    Task<EnderecoResponse> SaveAsync(Endereco endereco);

    Task<EnderecoResponse> UpdateAsync(int id, Endereco endereco);

    Task<EnderecoResponse> DeleteAsync(int id);
}
