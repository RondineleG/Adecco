namespace Adecco.Core.Interfaces.Services;

public interface IContatoService
{
    Task<IEnumerable<Contato>> ListAsync(int? clienteId, int? contatoId);

    Task<Contato> FindByIdAsync(int id);

    Task<ContatoResponse> SaveAsync(Contato contato);

    Task<ContatoResponse> UpdateAsync(int id, Contato contato);

    Task<ContatoResponse> DeleteAsync(int id);
}
