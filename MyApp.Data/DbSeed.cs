using MyApp.Data.Models;

namespace MyApp.Data;

public static class DbSeed
{
    public static void Seed(AppDbContext db)
    {
        if (db.Clients.Any() || db.Items.Any() || db.Rentals.Any())
            return;

        var clients = new List<Client>
        {
            new() { FullName = "Иванов Иван",      Phone = "+7 900 111-11-11", IsRegular = true  },
            new() { FullName = "Петрова Анна",     Phone = "+7 900 222-22-22", IsRegular = false },
            new() { FullName = "Сидоров Пётр",     Phone = "+7 900 333-33-33", IsRegular = false },
            new() { FullName = "Кузнецова Мария",  Phone = "+7 900 444-44-44", IsRegular = true  },
        };

        var items = new List<Item>
        {
            new() { Name = "Сапборд 10'6\"",       Size = "10'6\"", BuoyancyLevel = 4 },
            new() { Name = "Сапборд 11'0\"",       Size = "11'0\"", BuoyancyLevel = 5 },
            new() { Name = "Гидрокостюм",          Size = "M",      BuoyancyLevel = 2 },
            new() { Name = "Спасательный жилет",   Size = "L",      BuoyancyLevel = 5 },
        };

        db.Clients.AddRange(clients);
        db.Items.AddRange(items);
        db.SaveChanges();

        var rentals = new List<Rental>
        {
            new()
            {
                ClientId = clients[0].Id, ItemId = items[0].Id,
                StartDate = new DateTime(2026, 5, 1, 10, 0, 0),
                EndDate   = new DateTime(2026, 5, 1, 12, 30, 0),
                TotalPrice = 1500
            },
            new()
            {
                ClientId = clients[1].Id, ItemId = items[2].Id,
                StartDate = new DateTime(2026, 5, 2, 11, 0, 0),
                EndDate   = null,
                TotalPrice = 800
            },
            new()
            {
                ClientId = clients[2].Id, ItemId = items[1].Id,
                StartDate = new DateTime(2026, 5, 3, 9, 30, 0),
                EndDate   = new DateTime(2026, 5, 3, 13, 0, 0),
                TotalPrice = 2000
            },
            new()
            {
                ClientId = clients[3].Id, ItemId = items[3].Id,
                StartDate = new DateTime(2026, 5, 4, 15, 0, 0),
                EndDate   = null,
                TotalPrice = 500
            },
        };

        db.Rentals.AddRange(rentals);
        db.SaveChanges();
    }
}