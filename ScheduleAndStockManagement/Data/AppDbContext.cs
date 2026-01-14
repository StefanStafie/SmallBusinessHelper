using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data;

public class AppDbContext : DbContext
{
    private readonly string _dbPath;

    public DbSet<Inventory> InventoryItems { get; set; }
    public DbSet<Appointment> AppointmentItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<AppointmentType> AppointmentTypes { get; set; }

    public AppDbContext()
    {
        var folder = Environment.CurrentDirectory;
        _dbPath = Path.Combine(folder, "app.db");
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Filename={_dbPath}");
    }
}
