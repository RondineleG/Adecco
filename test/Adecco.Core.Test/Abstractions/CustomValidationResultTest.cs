namespace Adecco.Core.Test.Abstractions;

public class CustomValidationResultTest
{
    private readonly CustomValidationResult _validationResult;

    public CustomValidationResultTest()
    {
        _validationResult = new CustomValidationResult();
    }

    [Fact]
    public void Deve_Inicializar_Corretamente()
    {
        _validationResult.Errors.Should().BeEmpty();
        _validationResult.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("Erro de validação", "", "Erro de validação")]
    [InlineData("Erro de validação", "CampoTeste", "CampoTeste: Erro de validação")]
    public void AdicionarErro_Deve_Funcionar_Corretamente(
        string errorMessage,
        string fieldName,
        string expectedError
    )
    {
        _validationResult.AddError(errorMessage, fieldName);
        _validationResult.Errors.Should().ContainSingle().And.Contain(expectedError);
        _validationResult.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(true, "Erro condicional", "")]
    [InlineData(true, "Erro condicional", "CampoCondicional")]
    public void AdicionarErroSe_Deve_Funcionar_Corretamente(
        bool condition,
        string errorMessage,
        string fieldName
    )
    {
        _validationResult.AddErrorIf(condition, errorMessage, fieldName);
        var expectedError = string.IsNullOrWhiteSpace(fieldName)
            ? errorMessage
            : $"{fieldName}: {errorMessage}";
        _validationResult.Errors.Should().ContainSingle().And.Contain(expectedError);
        _validationResult.IsValid.Should().BeFalse();
    }

    [Fact]
    public void AdicionarErro_ComMensagemVazia_Deve_IgnorarErro()
    {
        _validationResult.AddError(string.Empty);
        _validationResult.Errors.Should().BeEmpty();
        _validationResult.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Mesclar_Deve_Consolidar_Erros_De_Dois_Resultados_De_Validacao()
    {
        var validationResult1 = new CustomValidationResult().AddError("Erro 1");
        var validationResult2 = new CustomValidationResult().AddError("Erro 2");
        validationResult1.Merge(validationResult2);
        validationResult1.Errors.Should().HaveCount(2).And.Contain("Erro 1").And.Contain("Erro 2");
        validationResult1.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Erros_Deve_Ser_Imutavel()
    {
        _validationResult.AddError("Erro inicial");
        var errorsBeforeModification = _validationResult.Errors.ToList();
        _validationResult.AddError("Erro adicional");
        errorsBeforeModification.Should().HaveCount(1).And.Contain("Erro inicial");
        _validationResult
            .Errors.Should()
            .HaveCount(2)
            .And.Contain(new[] { "Erro inicial", "Erro adicional" });
    }
}
