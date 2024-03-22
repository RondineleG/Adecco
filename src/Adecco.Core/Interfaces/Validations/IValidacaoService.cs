namespace Adecco.Core.Interfaces.Validations;

public interface IValidacaoService
{
    CustomValidationResult ValidarEmail(string email);

    CustomValidationResult ValidarCPF(string cpf);

    CustomValidationResult ValidarRG(string rg);

    CustomValidationResult ValidarCEP(string cep);

    CustomValidationResult ValidarTelefone(string telefone, ETipoContato tipoContato, int ddd);

    CustomValidationResult ValidarContato(Contato contato);

    CustomValidationResult ValidarEndereco(Endereco endereco);

    CustomValidationResult ValidarCliente(Cliente cliente);

    void AdicionarErroSeInvalido(
        CustomValidationResult resultado,
        string contexto,
        CustomResponse response
    );

    void Validar<T>(
        T entidade,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResponse response
    );

    void Validar<T>(
        IEnumerable<T> entidades,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResponse response
    );
}
