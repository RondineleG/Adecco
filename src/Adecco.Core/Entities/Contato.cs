using Adecco.Core.Entities.Base;

using System.Text.Json.Serialization;

namespace Adecco.Core.Entities;

public sealed class Contato : BaseEntity
{
    private readonly string tipoContato;
    private readonly int tipoContato1;

    [JsonConstructor]
    public Contato()
    { }

    public Contato(int id, string nome, int dDD, decimal telefone, ETipoContato tipoContato)
    {
        Id = id;
        Nome = nome;
        DDD = dDD;
        Telefone = telefone;
        TipoContato = tipoContato;
    }

    public Contato(string nome, int dDD, decimal telefone, ETipoContato tipoContato)
    {
        Nome = nome;
        DDD = dDD;
        Telefone = telefone;
        TipoContato = tipoContato;
    }

    public string Nome { get; private set; }

    public int DDD { get; private set; }

    public decimal Telefone { get; private set; }

    public int? ClienteId { get; private set; }

    public Cliente Cliente { get; private set; }

    public ETipoContato TipoContato { get; private set; }

    public void AlterarNome(string nome)
    {
        Nome = nome;
    }

    public void AtualizarDados(int id, string nome, int ddd, decimal telefone, string tipoContato)
    {
        Id = id;
        Nome = (!string.IsNullOrEmpty(nome)) ? nome : Nome;
        DDD = (ddd != int.MinValue) ? ddd : DDD;
        Telefone = (telefone != decimal.MinValue) ? telefone : Telefone;
        TipoContato = (ETipoContato)Enum.Parse(typeof(ETipoContato), tipoContato);
    }

    public void AdicionarClienteId(int clienteId)
    {
        ClienteId = clienteId;
    }

    public void AdicionarId(int id)
    {
        Id = id;
    }
}