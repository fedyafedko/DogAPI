﻿using Entities;

namespace Common.ExtensionMethods;

public static class Sort
{
    public static IQueryable<Dog> OrderByAttribute(this IQueryable<Dog> source, string attribute, string order)
    {
        switch (attribute.ToLower())
        {
            case "name":
                return order.ToLower() == "asc" ? source.OrderBy(d => d.Name) : source.OrderByDescending(d => d.Name);
            case "color":
                return order.ToLower() == "asc" ? source.OrderBy(d => d.Color) : source.OrderByDescending(d => d.Color);
            case "weight":
                return order.ToLower() == "asc" ? source.OrderBy(d => d.Weight) : source.OrderByDescending(d => d.Weight);
            case "tail_length":
                return order.ToLower() == "asc" ? source.OrderBy(d => d.TailLength) : source.OrderByDescending(d => d.TailLength);
            default:
                throw new ArgumentException("Invalid attribute for sorting");
        }
    }
}