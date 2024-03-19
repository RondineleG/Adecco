using System.Text.Json.Serialization;
using Adecco.Core.Entities.Base;

namespace Adecco.Core.Entities;

public sealed class Endereco : BaseEntity
{

    [JsonConstructor]
    public Endereco() { }

    public string Nome { get; private set; }

    public string CEP { get; private set; }

    public string Logradouro { get; private set; }

    public string Numero { get; private set; }

    public string Bairro { get; private set; }

    public string Complemento { get; private set; }

    public string Cidade { get; private set; }

    public string Estado { get; private set; }
    public string Referencia { get; private set; }

    public int? ClienteId { get; private set; }

    public Cliente Cliente { get; private set; }

    public ETipoEndereco TipoEndereco { get; private set; }

    public void AtualizarDados(
        int id,
        string nome,
        string cep,
        string logradouro,
        string numero,
        string bairro,
        string complemento,
        string cidade,
        string estado,
        string referencia,
        int tipoEndereco
    )
    {
        Id = id;
        Nome = (!string.IsNullOrEmpty(nome)) ? nome : Nome;
        CEP = (!string.IsNullOrEmpty(cep)) ? cep : CEP;
        Logradouro = (!string.IsNullOrEmpty(logradouro)) ? logradouro : Logradouro;
        Numero = (!string.IsNullOrEmpty(numero)) ? numero : Numero;
        Bairro = (!string.IsNullOrEmpty(bairro)) ? bairro : Bairro;
        Complemento = (!string.IsNullOrEmpty(complemento)) ? complemento : Complemento;
        Cidade = (!string.IsNullOrEmpty(cidade)) ? cidade : Cidade;
        Estado = (!string.IsNullOrEmpty(estado)) ? estado : Estado;
        Referencia = (!string.IsNullOrEmpty(referencia)) ? referencia : Referencia;
        TipoEndereco = (ETipoEndereco)tipoEndereco;
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
