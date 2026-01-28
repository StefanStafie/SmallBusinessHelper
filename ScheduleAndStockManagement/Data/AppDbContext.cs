using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data;

public class AppDbContext : DbContext
{
    private readonly string _dbPath;

    public DbSet<InventoryItemType> InventoryItemTypes { get; set; }
    public DbSet<InventoryItemTransaction> InventoryItemTransactions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<AppointmentType> AppointmentTypes { get; set; }
    public DbSet<Meeting> Meetings { get; set; }

    public AppDbContext()
    {
        string folder = Environment.CurrentDirectory;
        _dbPath = Path.Combine(folder, "app.db");

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseSqlite($"Filename={_dbPath}");
    }
}
