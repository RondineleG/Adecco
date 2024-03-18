namespace Adecco.Core.Interfaces.Repositories;

public interface IContatoRepository
{
    Task<IEnumerable<Contato>> ListAsync();

    Task AddAsync(Contato contato);

    Task<Contato> FindByIdAsync(int? id);

    void Update(Contato contato);

    void Remove(Contato contato);
}