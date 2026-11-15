using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Data.Models;
using System;
using System.Linq;

namespace MyApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        LoadData();
    }

    // === ОБЩАЯ ЗАГРУЗКА ДАННЫХ ===
    private void LoadData()
    {
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
            DbSeed.Seed(db);
        }

        LoadClients();
        LoadItems();
        LoadRentals();
        LoadCombos();
    }

    private void LoadClients()
    {
        using var db = new AppDbContext();
        ClientsList.ItemsSource = db.Clients.ToList();
    }

    private void LoadItems()
    {
        using var db = new AppDbContext();
        ItemsList.ItemsSource = db.Items.ToList();
    }

    private void LoadRentals()
    {
        using var db = new AppDbContext();
        RentalsList.ItemsSource = db.Rentals
            .Include(r => r.Client)
            .Include(r => r.Item)
            .ToList();
    }

    private void LoadCombos()
    {
        using var db = new AppDbContext();
        RentalClientCombo.ItemsSource = db.Clients.ToList();
        RentalItemCombo.ItemsSource = db.Items.ToList();
    }

    // === КЛИЕНТЫ ===
    private void AddClientBtn_Click(object? sender, RoutedEventArgs e)
    {
        ClientMessage.Text = "";
        if (string.IsNullOrWhiteSpace(ClientNameBox.Text) || string.IsNullOrWhiteSpace(ClientPhoneBox.Text))
        {
            ClientMessage.Text = "Ошибка: ФИО и телефон обязательны!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var client = new Client
            {
                FullName = ClientNameBox.Text,
                Phone = ClientPhoneBox.Text,
                IsRegular = ClientIsRegularCheck.IsChecked ?? false
            };
            db.Clients.Add(client);
            db.SaveChanges();

            ClientNameBox.Text = ""; ClientPhoneBox.Text = ""; ClientIsRegularCheck.IsChecked = false;
            LoadData();
        }
        catch (Exception ex)
        {
            ClientMessage.Text = $"Ошибка базы: {ex.Message}";
        }
    }

    private void DeleteClientBtn_Click(object? sender, RoutedEventArgs e)
    {
        ClientMessage.Text = "";
        if (ClientsList.SelectedItem is not Client selected)
        {
            ClientMessage.Text = "Выберите клиента для удаления.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var client = db.Clients.Find(selected.Id);
            if (client != null)
            {
                db.Clients.Remove(client);
                db.SaveChanges();
            }
            LoadData();
        }
        catch (Exception ex)
        {
            ClientMessage.Text = $"Ошибка удаления: {ex.Message}";
        }
    }

    private void ClientsList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (ClientsList.SelectedItem is Client selected)
        {
            ClientNameBox.Text = selected.FullName;
            ClientPhoneBox.Text = selected.Phone;
            ClientIsRegularCheck.IsChecked = selected.IsRegular;
        }
    }

    private void UpdateClientBtn_Click(object? sender, RoutedEventArgs e)
    {
        ClientMessage.Text = "";
        if (ClientsList.SelectedItem is not Client selected)
        {
            ClientMessage.Text = "Выберите клиента в списке для сохранения изменений.";
            return;
        }
        if (string.IsNullOrWhiteSpace(ClientNameBox.Text) || string.IsNullOrWhiteSpace(ClientPhoneBox.Text))
        {
            ClientMessage.Text = "Ошибка: ФИО и телефон не могут быть пустыми!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var client = db.Clients.Find(selected.Id);
            if (client != null)
            {
                client.FullName = ClientNameBox.Text;
                client.Phone = ClientPhoneBox.Text;
                client.IsRegular = ClientIsRegularCheck.IsChecked ?? false;
                db.SaveChanges();
            }
            LoadData();
        }
        catch (Exception ex)
        {
            ClientMessage.Text = $"Ошибка сохранения: {ex.Message}";
        }
    }

    // === СНАРЯЖЕНИЕ ===
    private void AddItemBtn_Click(object? sender, RoutedEventArgs e)
    {
        ItemMessage.Text = "";
        if (string.IsNullOrWhiteSpace(ItemNameBox.Text))
        {
            ItemMessage.Text = "Ошибка: Название обязательно!";
            return;
        }
        if (!int.TryParse(ItemBuoyancyBox.Text, out int buoyancy))
        {
            ItemMessage.Text = "Ошибка: Плавучесть должна быть целым числом!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var item = new Item
            {
                Name = ItemNameBox.Text,
                Size = string.IsNullOrWhiteSpace(ItemSizeBox.Text) ? null : ItemSizeBox.Text,
                BuoyancyLevel = buoyancy
            };
            db.Items.Add(item);
            db.SaveChanges();

            ItemNameBox.Text = ""; ItemSizeBox.Text = ""; ItemBuoyancyBox.Text = "";
            LoadData();
        }
        catch (Exception ex)
        {
            ItemMessage.Text = $"Ошибка базы: {ex.Message}";
        }
    }

    private void DeleteItemBtn_Click(object? sender, RoutedEventArgs e)
    {
        ItemMessage.Text = "";
        if (ItemsList.SelectedItem is not Item selected)
        {
            ItemMessage.Text = "Выберите снаряжение для удаления.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var item = db.Items.Find(selected.Id);
            if (item != null)
            {
                db.Items.Remove(item);
                db.SaveChanges();
            }
            LoadData();
        }
        catch (Exception ex)
        {
            ItemMessage.Text = $"Ошибка удаления: {ex.Message}";
        }
    }

    // === АРЕНДЫ ===
    private void AddRentalBtn_Click(object? sender, RoutedEventArgs e)
    {
        RentalMessage.Text = "";
        if (RentalClientCombo.SelectedItem is not Client selectedClient)
        {
            RentalMessage.Text = "Ошибка: Выберите клиента!";
            return;
        }
        if (RentalItemCombo.SelectedItem is not Item selectedItem)
        {
            RentalMessage.Text = "Ошибка: Выберите снаряжение!";
            return;
        }
        if (RentalStartDatePicker.SelectedDate == null)
        {
            RentalMessage.Text = "Ошибка: Выберите дату начала!";
            return;
        }
        if (!int.TryParse(RentalPriceBox.Text, out int price))
        {
            RentalMessage.Text = "Ошибка: Сумма должна быть целым числом!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var rental = new Rental
            {
                // ВАЖНО: записываем именно Id, а не объект целиком!
                ClientId = selectedClient.Id,
                ItemId = selectedItem.Id,
                StartDate = RentalStartDatePicker.SelectedDate.Value,
                EndDate = RentalEndDatePicker.SelectedDate,
                TotalPrice = price
            };
            db.Rentals.Add(rental);
            db.SaveChanges();

            RentalPriceBox.Text = "";
            RentalStartDatePicker.SelectedDate = null;
            RentalEndDatePicker.SelectedDate = null;
            LoadData();
        }
        catch (Exception ex)
        {
            RentalMessage.Text = $"Ошибка базы: {ex.Message}";
        }
    }

    private void DeleteRentalBtn_Click(object? sender, RoutedEventArgs e)
    {
        RentalMessage.Text = "";
        if (RentalsList.SelectedItem is not Rental selected)
        {
            RentalMessage.Text = "Выберите аренду для удаления.";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var rental = db.Rentals.Find(selected.Id);
            if (rental != null)
            {
                db.Rentals.Remove(rental);
                db.SaveChanges();
            }
            LoadData();
        }
        catch (Exception ex)
        {
            RentalMessage.Text = $"Ошибка удаления: {ex.Message}";
        }
    }
}