using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data
{
    public class InventoryItemTransactionRepository : IGeneralRepository<InventoryItemTransaction>
    {
        private readonly AppDbContext _context;

        public InventoryItemTransactionRepository(AppDbContext context)
        {
            _context = context;
            _ = _context.Database.EnsureCreated();
        }

        public async Task<List<InventoryItemTransaction>> GetAllAsync()
        {
            return await _context.InventoryItemTransactions.ToListAsync();
        }

        public async Task<InventoryItemTransaction> GetByIdAsync(int id)
        {
            return await _context.InventoryItemTransactions.FindAsync(id);
        }

        public async Task AddAsync(InventoryItemTransaction item)
        {
            _ = _context.InventoryItemTransactions.Add(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InventoryItemTransaction item)
        {
            _ = _context.InventoryItemTransactions.Update(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            InventoryItemTransaction? item = await _context.InventoryItemTransactions.FindAsync(id);
            if (item is null)
            {
                return;
            }

            _ = _context.InventoryItemTransactions.Remove(item);
            _ = await _context.SaveChangesAsync();
        }
    }
}
