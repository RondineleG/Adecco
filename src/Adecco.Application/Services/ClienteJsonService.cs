namespace Adecco.Application.Services;

public sealed class ClienteJsonService : IClienteJsonService
{
    public ClienteJsonService(IClienteJsonRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    private readonly IClienteJsonRepository _clienteRepository;

    public List<Cliente> ListarClientes(string nome, string email, string cpf)
    {
        return _clienteRepository
            .ObterTodos()
            .Where(c =>
                (string.IsNullOrEmpty(nome) || c.Nome.Contains(nome))
                && (string.IsNullOrEmpty(email) || c.Email == email)
                && (string.IsNullOrEmpty(cpf) || c.CPF == cpf)
            )
            .ToList();
    }

    public void AdicionarCliente(Cliente cliente)
    {
        _clienteRepository.Adicionar(cliente);
    }

    public void AtualizarCliente(int id, Cliente clienteAtualizado)
    {
        _clienteRepository.Atualizar(id, clienteAtualizado);
    }

    public void RemoverCliente(int id)
    {
        _clienteRepository.Remover(id);
    }

    public void AtualizarContato(int clienteId, Contato contato)
    {
        _clienteRepository.AtualizarContato(clienteId, contato);
    }

    public void AtualizarEndereco(int clienteId, Endereco endereco)
    {
        _clienteRepository.AtualizarEndereco(clienteId, endereco);
    }

    public void RemoverContato(int clienteId, int contatoId)
    {
        _clienteRepository.RemoverContato(clienteId, contatoId);
    }

    public void RemoverEndereco(int clienteId, int enderecoId)
    {
        _clienteRepository.RemoverEndereco(clienteId, enderecoId);
    }

    public void IncluirContato(int clienteId, Contato contato)
    {
        _clienteRepository.IncluirContato(clienteId, contato);
    }

    public void IncluirEndereco(int clienteId, Endereco endereco)
    {
        _clienteRepository.IncluirEndereco(clienteId, endereco);
    }

    public Cliente BuscarClientePodId(int clienteId)
    {
        return _clienteRepository.ObterPorId(clienteId);
    }
}
