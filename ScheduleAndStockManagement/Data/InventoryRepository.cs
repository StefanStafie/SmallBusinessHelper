using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data
{
    public class InventoryRepository : IGeneralRepository<Inventory>
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
            _ = _context.Database.EnsureCreated();
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.InventoryItems.ToListAsync();
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            return await _context.InventoryItems.FindAsync(id);
        }

        public async Task AddAsync(Inventory item)
        {
            _ = _context.InventoryItems.Add(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inventory item)
        {
            _ = _context.InventoryItems.Update(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Inventory? item = await _context.InventoryItems.FindAsync(id);
            if (item is null)
            {
                return;
            }

            _ = _context.InventoryItems.Remove(item);
            _ = await _context.SaveChangesAsync();
        }
    }
}
