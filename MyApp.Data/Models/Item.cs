namespace MyApp.Data.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Size { get; set; }
    public int BuoyancyLevel { get; set; }

    public List<Rental> Rentals { get; set; } = new();

    public override string ToString()
        => $"{Name}, размер: {Size ?? "—"}, плавучесть: {BuoyancyLevel}";
}