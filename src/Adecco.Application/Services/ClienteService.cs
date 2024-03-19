namespace Adecco.Application.Services;

public sealed class ClienteService(
    IClienteRepository productRepository,
    IContatoService contatoService,
    IUnitOfWork unitOfWork,
    IEnderecoService enderecoService,
    IValidacaoService validacaoService
    ) : IClienteService
{
    private readonly IClienteRepository _clienteRepository = productRepository;
    private readonly IContatoService _contatoService = contatoService;
    private readonly IEnderecoService _enderecoService = enderecoService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidacaoService _validacaoService = validacaoService;
    private readonly List<string> _errosValidacao = new List<string>();

    public async Task<IEnumerable<Cliente>> ListAsync(string? nome, string? email, string? cpf)
    {
        return await _clienteRepository.ListAsync(nome, email, cpf);
    }

    public async Task<Cliente> FindByIdAsync(int id)
    {
        return await _clienteRepository.FindByIdAsync(id);
    }

    public async Task<ClienteResponse> SaveAsync(Cliente cliente)
    {
        try
        {
            foreach (var contato in cliente.Contatos)
            {
                var contatoExistente = await _contatoService.FindByIdAsync(contato.Id);
                if (contatoExistente == null)
                {
                    return new ClienteResponse("Contato invalido.");
                }
            }

            foreach (var endereco in cliente.Enderecos)
            {
                var enderecoExistente = await _enderecoService.FindByIdAsync(endereco.Id);
                if (enderecoExistente == null)
                {
                    return new ClienteResponse("Endereco invalido.");
                }
            }
            var response = new CustomResponse();
            _validacaoService.Validar(
                cliente,
                _validacaoService.ValidarCliente,
                "Cliente",
                response
            );
            _validacaoService.Validar(
                cliente.Contatos,
                _validacaoService.ValidarContato,
                "Contato",
                response
            );
            _validacaoService.Validar(
                cliente.Enderecos,
                _validacaoService.ValidarEndereco,
                "Endere�o",
                response
            );
            if (!response.Success)
            {
                return new ClienteResponse(response.ToString());
            }

            await _unitOfWork.BeginTransactionAsync();
            await _clienteRepository.AddAsync(cliente);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();
            return new ClienteResponse(cliente);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ClienteResponse($"Ocorreu um erro: {ex.Message}");
        }
    }

    public async Task<ClienteResponse> DeleteAsync(int id)
    {
        var clienteExistente = await _clienteRepository.FindByIdAsync(id);
        if (clienteExistente == null)
        {
            return new ClienteResponse("Cliente n�o encontrado.");
        }

        try
        {
            _clienteRepository.Remove(clienteExistente);
            await _unitOfWork.CompleteAsync();
            return new ClienteResponse(clienteExistente);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ClienteResponse($"Ocorreu um erro ao excluir o cliente: {ex.Message}");
        }
    }

    public async Task<ClienteResponse> UpdateAsync(int clienteId, Cliente cliente)
    {
        var clienteExistente = await _clienteRepository.FindByIdAsync(clienteId);
        if (clienteExistente == null)
        {
            return new ClienteResponse("Cliente n�o encontrado.");
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await AtualizarContato(cliente, clienteExistente);
            await AtualizarEndereco(cliente, clienteExistente);
            clienteExistente.AtualizarCliente(
                clienteExistente.Id,
                cliente.Nome,
                cliente.Email,
                cliente.CPF,
                cliente.RG
            );
            _clienteRepository.Update(clienteExistente);

            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();

            return new ClienteResponse(clienteExistente);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ClienteResponse($"Ocorreu um erro ao atualizar o cliente: {ex.Message}");
        }
    }

    private async Task AtualizarContato(Cliente cliente, Cliente clienteExistente)
    {
        var contatosARemover = clienteExistente
            .Contatos.Where(c => !cliente.Contatos.Any(nc => nc.Id == c.Id))
            .ToList();
        foreach (var contato in contatosARemover)
        {
            await _contatoService.DeleteAsync(contato.Id);
        }
        foreach (var contatoNovo in cliente.Contatos)
        {
            var contatoExistente = clienteExistente.Contatos.FirstOrDefault(c =>
                c.Id == contatoNovo.Id
            );
            if (contatoExistente != null)
            {
                await _contatoService.UpdateAsync(contatoExistente.Id, contatoNovo);
            }
            else
            {
                contatoNovo.AdicionarClienteId(clienteExistente.Id);
                await _contatoService.SaveAsync(contatoNovo);
            }
        }
    }

    private async Task AtualizarEndereco(Cliente cliente, Cliente clienteExistente)
    {
        var enderecosARemover = clienteExistente
            .Enderecos.Where(e => !cliente.Enderecos.Any(ne => ne.Id == e.Id))
            .ToList();
        foreach (var endereco in enderecosARemover)
        {
            await _enderecoService.DeleteAsync(endereco.Id);
        }

        foreach (var enderecoNovo in cliente.Enderecos)
        {
            var enderecoExistente = clienteExistente.Enderecos.FirstOrDefault(e =>
                e.Id == enderecoNovo.Id
            );
            if (enderecoExistente != null)
            {
                await _enderecoService.UpdateAsync(enderecoExistente.Id, enderecoNovo);
            }
            else
            {
                enderecoNovo.AdicionarClienteId(clienteExistente.Id);
                await _enderecoService.SaveAsync(enderecoNovo);
            }
        }
    }

    private void Validar<T>(
        T entidade,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade
    )
    {
        var resultado = funcValidacao(entidade);
        AdicionarErroSeInvalido(resultado, nomeEntidade);
    }

    private void AdicionarErroSeInvalido(CustomValidationResult resultado, string contexto = null)
    {
        if (!resultado.IsValid)
        {
            var prefixo = string.IsNullOrEmpty(contexto) ? "" : $"{contexto}: ";
            _errosValidacao.AddRange(resultado.Errors.Select(erro => prefixo + erro));
        }
    }
}
