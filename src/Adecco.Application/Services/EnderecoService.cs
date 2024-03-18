namespace Adecco.Application.Services;

public sealed class EnderecoService : IEnderecoService
{
    public EnderecoService(IEnderecoRepository enderecoRepository, IUnitOfWork unitOfWork)
    {
        _enderecoRepository = enderecoRepository;
        _unitOfWork = unitOfWork;
    }

    private readonly IEnderecoRepository _enderecoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public async Task<IEnumerable<Endereco>> ListAsync(int? clienteId, int? enderecoId)
    {
        return await _enderecoRepository.ListAsync(clienteId, enderecoId);
    }

    public async Task<Endereco> FindByIdAsync(int id)
    {
        return await _enderecoRepository.FindByIdAsync(id);
    }

    public async Task<EnderecoResponse> SaveAsync(Endereco endereco)
    {
        try
        {
            await _enderecoRepository.AddAsync(endereco);
            await _unitOfWork.CompleteAsync();
            return new EnderecoResponse(endereco);
        }
        catch (Exception ex)
        {
            return new EnderecoResponse($"Ocorreu um erro ao salvar o endereco: {ex.Message}");
        }
    }

    public async Task<EnderecoResponse> UpdateAsync(int id, Endereco endereco)
    {
        var enderecoExistente = await _enderecoRepository.FindByIdAsync(id);

        if (enderecoExistente == null)
        {
            return new EnderecoResponse("Endereco n�o encontrado.");
        }

        enderecoExistente.AtualizarDados(
            id,
            endereco.Nome,
            endereco.CEP,
            endereco.Logradouro,
            endereco.Numero,
            endereco.Bairro,
            endereco.Complemento,
            endereco.Cidade,
            endereco.Estado,
            endereco.Referencia,
            (int)endereco.TipoEndereco
        );

        try
        {
            _enderecoRepository.Update(enderecoExistente);
            await _unitOfWork.CompleteAsync();
            return new EnderecoResponse(enderecoExistente);
        }
        catch (Exception ex)
        {
            return new EnderecoResponse($"Ocorreu um erro ao atualizar o endereco: {ex.Message}");
        }
    }

    public async Task<EnderecoResponse> DeleteAsync(int id)
    {
        var existingCategory = await _enderecoRepository.FindByIdAsync(id);

        if (existingCategory == null)
        {
            return new EnderecoResponse("Endereco n�o encontrado.");
        }

        try
        {
            _enderecoRepository.Remove(existingCategory);
            await _unitOfWork.CompleteAsync();
            return new EnderecoResponse(existingCategory);
        }
        catch (Exception ex)
        {
            return new EnderecoResponse($"Ocorreu um erro ao excluir o endereco: {ex.Message}");
        }
    }
}