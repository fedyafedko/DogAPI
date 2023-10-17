using Common.Enums;

namespace Common.Extensions;

public static class EnumsExtensions
{
    public static Order ToOrder(this string? value)
    {
        return value?.ToLower() == "desc" ? Order.Descending : Order.Ascending;
    }

    public static TEnum? ToEnum<TEnum>(this string source) where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(source, ignoreCase: true, out var result))
            return result;
        
        return null;
    }
}