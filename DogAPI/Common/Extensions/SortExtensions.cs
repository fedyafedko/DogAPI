using System.Linq.Expressions;
using Common.Enums;
using Entities;

namespace Common.Extensions;

public static class SortExtensions
{
    public static IQueryable<Dog> OrderByAttribute(this IQueryable<Dog> source, DogOrderingProperty property, Order order)
    {
        Expression<Func<Dog, object>> keySelector = property switch
        {
            DogOrderingProperty.Name => dog => dog.Name,
            DogOrderingProperty.Color => dog => dog.Color,
            DogOrderingProperty.TailLength => dog => dog.TailLength,
            DogOrderingProperty.Weight => dog => dog.Weight,
            _ => throw new ArgumentException("Invalid attribute for sorting")
        };

        return order == Order.Ascending
            ? source.OrderBy(keySelector)
            : source.OrderByDescending(keySelector);
    }
}