namespace Adecco.Application.Services;

public class ContatoService : IContatoService
{
    public ContatoService(IContatoRepository contatoRepository, IUnitOfWork unitOfWork)
    {
        _contatoRepository = contatoRepository;
        _unitOfWork = unitOfWork;
    }

    private readonly IContatoRepository _contatoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<IEnumerable<Contato>> ListAsync(int? clienteId, int? contatoId)
    {
        return await _contatoRepository.ListAsync(clienteId, contatoId);
    }

    public async Task<Contato> FindByIdAsync(int id)
    {
        return await _contatoRepository.FindByIdAsync(id);
    }

    public async Task<ContatoResponse> SaveAsync(Contato contato)
    {
        try
        {
            await _contatoRepository.AddAsync(contato);
            await _unitOfWork.CompleteAsync();
            return new ContatoResponse(contato);
        }
        catch (Exception ex)
        {
            return new ContatoResponse($"Ocorreu um erro ao salvar o contato: {ex.Message}");
        }
    }

    public async Task<ContatoResponse> UpdateAsync(int id, Contato contato)
    {
        var contatoExistente = await _contatoRepository.FindByIdAsync(id);
        if (contatoExistente == null)
        {
            return new ContatoResponse("Contato n�o encontrado.");
        }

        contatoExistente.AtualizarDados(
            id,
            contato.Nome,
            contato.DDD,
            contato.Telefone,
            (int)contato.TipoContato
        );
        try
        {
            _contatoRepository.Update(contatoExistente);
            await _unitOfWork.CompleteAsync();
            return new ContatoResponse(contatoExistente);
        }
        catch (Exception ex)
        {
            return new ContatoResponse($"Ocorreu um erro ao atualizar o contato: {ex.Message}");
        }
    }

    public async Task<ContatoResponse> DeleteAsync(int id)
    {
        var contatoExistente = await _contatoRepository.FindByIdAsync(id);
        if (contatoExistente == null)
        {
            return new ContatoResponse("Contato n�o encontrado.");
        }

        try
        {
            _contatoRepository.Remove(contatoExistente);
            await _unitOfWork.CompleteAsync();
            return new ContatoResponse(contatoExistente);
        }
        catch (Exception ex)
        {
            return new ContatoResponse(
                $"Ocorreu um erro ao excluir o contatoExistente: {ex.Message}"
            );
        }
    }
}
