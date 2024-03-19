using System.Reflection;

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

    public static TEnum ParseEnumFromDescription<TEnum>(string description) where TEnum : struct
    {
        foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                {
                    if (Enum.TryParse<TEnum>(field.Name, out var value))
                    {
                        return value;
                    }
                }
            }
        }

        throw new ArgumentException($"Não foi possível encontrar um valor correspondente para '{description}' em {typeof(TEnum).Name}.");
    }
}