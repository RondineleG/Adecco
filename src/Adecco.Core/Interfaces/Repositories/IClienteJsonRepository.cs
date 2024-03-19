namespace Adecco.Core.Interfaces.Repositories;

public interface IClienteJsonRepository
{
    Task<List<Cliente>> ObterTodos();

    Task<IEnumerable<Cliente>> ListarClientes(string? nome, string? email, string? cpf);

    Task<Cliente> BuscarClientePodId(int clienteId);

    Task<ClienteResponse> AdicionarCliente(Cliente cliente);

    Task<ClienteResponse> AtualizarCliente(int id, Cliente cliente);

    void RemoverCliente(int id);

    Task<ContatoResponse> AtualizarContato(int clienteId, Contato contato);

    Task<EnderecoResponse> AtualizarEndereco(int clienteId, Endereco endereco);

    void RemoverContato(int clienteId, int contatoId);

    void RemoverEndereco(int clienteId, int enderecoId);

    Task<ContatoResponse> IncluirContato(int clienteId, Contato contato);

    Task<EnderecoResponse> IncluirEndereco(int clienteId, Endereco endereco);
}