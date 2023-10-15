using System.ComponentModel.DataAnnotations;
using Entities.Attributes;

namespace Entities;

public class Dog
{
    [Key]
    [BaseSort]
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public int TailLength { get; set; }
    public int Weight { get; set; }
}