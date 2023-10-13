using Entities;

namespace Common.ExtensionMethods;

public static class Sort
{
    public static IQueryable<Dog> OrderByAttribute(this IQueryable<Dog> source, string attribute, string order)
    {
        switch (attribute.ToLower())
        {
            case "weight":
                return order.ToLower() == "asc" ? source.OrderBy(d => d.Weight) : source.OrderByDescending(d => d.Weight);
            default:
                throw new ArgumentException("Invalid attribute for sorting");
        }
    }
}