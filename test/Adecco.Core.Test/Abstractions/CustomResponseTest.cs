namespace Adecco.Core.Test.Abstractions;

public class CustomResponseTest
{
    [Fact]
    public void CustomResponse_Deve_Inicializar_Com_Sucesso_True()
    {
        var response = new CustomResponse();
        response.Status.Should().Be(CustomResultStatus.Success);
        response.GeneralErrors.Should().BeEmpty();
        response.EntityErrors.Should().BeEmpty();
    }

    [Fact]
    public void AddError_Deve_Adicionar_Erro_Geral_E_Alterar_Sucesso_Para_False()
    {
        var response = new CustomResponse();
        response.AddError("Erro geral");

        response.Status.Should().NotBe(CustomResultStatus.Success);
        response.GeneralErrors.Should().ContainSingle().And.Contain("Erro geral");
    }

    [Fact]
    public void AddEntityError_Deve_Adicionar_Erro_De_Entidade_E_Alterar_Sucesso_Para_False()
    {
        var response = new CustomResponse();
        response.AddEntityError("Entidade1", "Erro de entidade");

        response.Status.Should().NotBe(CustomResultStatus.Success);
        response.EntityErrors.Should().HaveCount(1);
        response.EntityErrors["Entidade1"].Should().ContainSingle().And.Contain("Erro de entidade");
    }

    [Fact]
    public void ToString_Deve_Retornar_Todos_Os_Erros_Formatados()
    {
        var response = new CustomResponse();
        response.AddError("Erro geral 1");
        response.AddEntityError("Entidade1", "Erro de entidade 1");
        response.AddEntityError("Entidade2", "Erro de entidade 2");

        var expectedOutput =
            "Erro geral 1; Entidade1: Erro de entidade 1; Entidade2: Erro de entidade 2";
        response.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public void ToString_Deve_Retornar_String_Vazia_Se_Nao_Houver_Erros()
    {
        var response = new CustomResponse();
        response.ToString().Should().BeEmpty();
    }
}
