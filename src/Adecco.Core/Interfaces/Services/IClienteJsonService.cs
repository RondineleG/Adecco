namespace Adecco.Core.Interfaces.Services;

public interface IClienteJsonService
{
    List<Cliente> ListarClientes(string nome, string email, string cpf);
    Cliente BuscarClientePodId(int clienteId);

    void AdicionarCliente(Cliente cliente);

    void AtualizarCliente(int id, Cliente cliente);

    void RemoverCliente(int id);

    void AtualizarContato(int clienteId, Contato contato);

    void AtualizarEndereco(int clienteId, Endereco endereco);

    void RemoverContato(int clienteId, int contatoId);

    void RemoverEndereco(int clienteId, int enderecoId);

    void IncluirContato(int clienteId, Contato contato);

    void IncluirEndereco(int clienteId, Endereco endereco);
}
