namespace Adecco.Application.Services;

public sealed class ClienteJsonService(IClienteJsonRepository clienteRepository) : IClienteJsonService
{
    private readonly IClienteJsonRepository _clienteRepository = clienteRepository;

    public async Task<IEnumerable<Cliente>> ListarClientes(string? nome, string? email, string? cpf)
    {
        return await _clienteRepository.ListarClientes(nome, email, cpf);
    }

    public async Task<ClienteResponse> AdicionarCliente(Cliente cliente)
    {
        return await _clienteRepository.AdicionarCliente(cliente);
    }

    public async Task<ClienteResponse> AtualizarCliente(int id, Cliente clienteAtualizado)
    {
        return await _clienteRepository.AtualizarCliente(id, clienteAtualizado);
    }

    public void RemoverCliente(int id)
    {
        _clienteRepository.RemoverCliente(id);
    }

    public async Task<ContatoResponse> AtualizarContato(int clienteId, Contato contato)
    {
        return await _clienteRepository.AtualizarContato(clienteId, contato);
    }

    public async Task<EnderecoResponse> AtualizarEndereco(int clienteId, Endereco endereco)
    {
        return await _clienteRepository.AtualizarEndereco(clienteId, endereco);
    }

    public void RemoverContato(int clienteId, int contatoId)
    {
        _clienteRepository.RemoverContato(clienteId, contatoId);
    }

    public void RemoverEndereco(int clienteId, int enderecoId)
    {
        _clienteRepository.RemoverEndereco(clienteId, enderecoId);
    }

    public async Task<ContatoResponse> IncluirContato(int clienteId, Contato contato)
    {
        return await _clienteRepository.IncluirContato(clienteId, contato);
    }

    public async Task<EnderecoResponse> IncluirEndereco(int clienteId, Endereco endereco)
    {
        return await _clienteRepository.IncluirEndereco(clienteId, endereco);
    }

    public async Task<Cliente> BuscarClientePodId(int clienteId)
    {
        return await _clienteRepository.BuscarClientePodId(clienteId);
    }
}
