namespace Adecco.Core.Interfaces.Repositories;

public interface IEnderecoRepository
{
    Task<IEnumerable<Endereco>> ListAsync(int? clienteId, int? enderecoId);

    Task AddAsync(Endereco endereco);

    Task<Endereco> FindByIdAsync(int? id);

    void Update(Endereco endereco);

    void Remove(Endereco endereco);
}