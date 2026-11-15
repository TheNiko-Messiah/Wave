namespace MyApp.Data.Models;

public class Rental
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TotalPrice { get; set; }

    public override string ToString()
    {
        var end = EndDate.HasValue ? EndDate.Value.ToString("dd.MM.yyyy HH:mm") : "ещё на руках";
        return $"{Client?.FullName ?? "?"} — {Item?.Name ?? "?"} | {StartDate:dd.MM.yyyy HH:mm} → {end} | {TotalPrice} ₽";
    }
}