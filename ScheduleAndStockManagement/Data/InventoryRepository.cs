using ScheduleAndStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ScheduleAndStockManagement.Data
{
    public class InventoryRepository : IGeneralRepository<Inventory>
    {
        private readonly AppDbContext _context;

        public InventoryRepository(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
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
            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inventory item)
        {
            _context.InventoryItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item is null) return;
            _context.InventoryItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
