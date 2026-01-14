using ScheduleAndStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ScheduleAndStockManagement.Data
{
    public class CustomerRepository : IGeneralRepository<Customer>
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer item)
        {
            _context.Customers.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer item)
        {
            _context.Customers.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.Customers.FindAsync(id);
            if (item is null) return;
            _context.Customers.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
