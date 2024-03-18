namespace Adecco.Core.Interfaces.Repositories;

public interface IClienteJsonRepository
{
    List<Cliente> ObterTodos();

    Cliente ObterPorId(int id);

    void Adicionar(Cliente cliente);

    void Atualizar(int id, Cliente cliente);

    void Remover(int id);

    void AtualizarContato(int clienteId, Contato contato);

    void AtualizarEndereco(int clienteId, Endereco endereco);

    void RemoverContato(int clienteId, int contatoId);

    void RemoverEndereco(int clienteId, int enderecoId);

    void IncluirContato(int clienteId, Contato contato);

    void IncluirEndereco(int clienteId, Endereco endereco);
}
