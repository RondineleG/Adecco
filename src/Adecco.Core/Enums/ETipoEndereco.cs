namespace Adecco.Core.Enums;

public enum ETipoEndereco : byte
{
    [Description("Preferencial")]
    Preferencial = 1,

    [Description("Entrega")]
    Entrega = 2,

    [Description("Cobrança")]
    Cobrança = 3,
}