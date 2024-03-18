namespace Adecco.Application.Extensions;

public static class EnumExtensions
{
    public static string ToDescriptionString<TEnum>(this TEnum @enum)
    {
        var info = @enum.GetType().GetField(@enum.ToString());
        var attributes = (DescriptionAttribute[])
            info.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return (attributes?[0].Description) ?? @enum.ToString();
    }
}
