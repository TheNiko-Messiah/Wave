using Microsoft.EntityFrameworkCore;
using MyApp.Data;

Console.WriteLine($"Файл базы: {AppDbContext.DbPath}");
Console.WriteLine();

using (var db = new AppDbContext())
{
    db.Database.EnsureCreated();
    DbSeed.Seed(db);
}

Console.WriteLine("=== Клиенты ===");
using (var db = new AppDbContext())
{
    foreach (var c in db.Clients.ToList())
        Console.WriteLine(c);
}

Console.WriteLine();
Console.WriteLine("=== Снаряжение ===");
using (var db = new AppDbContext())
{
    foreach (var i in db.Items.ToList())
        Console.WriteLine(i);
}

Console.WriteLine();
Console.WriteLine("=== Аренды ===");
using (var db = new AppDbContext())
{
    var rentals = db.Rentals
        .Include(r => r.Client)
        .Include(r => r.Item)
        .ToList();

    foreach (var r in rentals)
        Console.WriteLine(r);
}