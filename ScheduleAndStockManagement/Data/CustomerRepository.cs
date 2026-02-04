using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data
{
    public class CustomerRepository : IGeneralRepository<Customer>
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
            _ = _context.Database.EnsureCreated();
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _context.Customers.ToListAsync();

            foreach (var customer in customers)
            {
                customer.CustomerFiles = await _context.CustomerFiles
                    .Where(f => f.CustomerId == customer.Id)
                    .ToListAsync();
            }

            return customers;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task AddAsync(Customer item)
        {
            _ = _context.Customers.Add(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer item)
        {
            _ = _context.Customers.Update(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Customer? item = await _context.Customers.FindAsync(id);
            if (item is null)
            {
                return;
            }

            _ = _context.Customers.Remove(item);
            _ = await _context.SaveChangesAsync();
        }
    }
}
