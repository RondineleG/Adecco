using System.ComponentModel;
using Adecco.Application.Extensions;

namespace Adecco.Application.Test.Extensions;

public enum MeuEnum
{
    [Description("Primeira descrição")]
    PrimeiroValor,

    [Description("Segunda descrição")]
    SegundoValor,
    TerceiroValor
}

public class EnumExtensionsTest
{
    [Fact]
    public void ToDescriptionString_DeveRetornarDescricaoCorreta()
    {
        var primeiroEnum = MeuEnum.PrimeiroValor;
        var segundoEnum = MeuEnum.SegundoValor;
        var terceiroEnum = MeuEnum.TerceiroValor;

        var descricaoPrimeiro = primeiroEnum.ToDescriptionString();
        var descricaoSegundo = segundoEnum.ToDescriptionString();
        var descricaoTerceiro = terceiroEnum.ToDescriptionString();

        descricaoPrimeiro.Should().Be("Primeira descrição");
        descricaoSegundo.Should().Be("Segunda descrição");
        descricaoTerceiro.Should().Be("TerceiroValor");
    }

    [Fact]
    public void ToDescriptionString_Com_Enum_Null_Deve_Lancar_Excecao()
    {
        MeuEnum? meuEnum = null;
        Action act = () => meuEnum.ToDescriptionString();
        act.Should().Throw<ArgumentNullException>();
    }
}
