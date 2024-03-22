namespace Adecco.Core.Interfaces.Repositories;

public interface IContatoRepository
{
    Task<IEnumerable<Contato>> ListAsync(int? clienteId, int? contatoId);

    Task AddAsync(Contato contato);

    Task<Contato> FindByIdAsync(int? id);

    void Update(Contato contato);

    void Remove(Contato contato);
}
