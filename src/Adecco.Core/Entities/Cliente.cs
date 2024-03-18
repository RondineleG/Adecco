using System.Text.Json.Serialization;

namespace Adecco.Core.Entities;

public sealed class Cliente : BaseEntity
{
    [JsonConstructor]
    public Cliente()
    {
        Contatos = new List<Contato>();
        Enderecos = new List<Endereco>();
    }

    public string Nome { get; private set; }

    public string Email { get; private set; }

    public string CPF { get; private set; }

    public string RG { get; private set; }

    public int ContatoId { get; private set; }

    public int EnderecoId { get; private set; }

    public List<Contato> Contatos { get; private set; }

    public List<Endereco> Enderecos { get; private set; }

    public void AdicionarContato(Contato contato)
    {
        Contatos.Add(contato);
    }

    public void AdicionarEndereco(Endereco endereco)
    {
        Enderecos.Add(endereco);
    }

    public void AtualizarCliente(int id, string nome, string email, string cpf, string rg)
    {
        Id = id;
        Nome = nome ?? Nome;
        Email = email ?? Email;
        CPF = cpf ?? CPF;
        RG = rg ?? RG;
    }

    public void AdicionarId(int id)
    {
        Id = id;
    }
}
