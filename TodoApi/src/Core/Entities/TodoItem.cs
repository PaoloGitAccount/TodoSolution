namespace Core.Entities;
using System;

public record TodoItem
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public int Priority { get; init; } = 3;
    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
    public DateTime? DueDate { get; init; }
}
