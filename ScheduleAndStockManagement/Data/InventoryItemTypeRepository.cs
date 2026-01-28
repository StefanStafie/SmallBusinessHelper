using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data
{
    public class InventoryItemTypeRepository : IGeneralRepository<InventoryItemType>
    {
        private readonly AppDbContext _context;

        public InventoryItemTypeRepository(AppDbContext context)
        {
            _context = context;
            _ = _context.Database.EnsureCreated();
        }

        public async Task<List<InventoryItemType>> GetAllAsync()
        {
            return await _context.InventoryItemTypes.ToListAsync();
        }

        public async Task<InventoryItemType> GetByIdAsync(int id)
        {
            return await _context.InventoryItemTypes.FindAsync(id);
        }

        public async Task AddAsync(InventoryItemType item)
        {
            _ = _context.InventoryItemTypes.Add(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InventoryItemType item)
        {
            _ = _context.InventoryItemTypes.Update(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            InventoryItemType? item = await _context.InventoryItemTypes.FindAsync(id);
            if (item is null)
            {
                return;
            }

            _ = _context.InventoryItemTypes.Remove(item);
            _ = await _context.SaveChangesAsync();
        }
    }
}
