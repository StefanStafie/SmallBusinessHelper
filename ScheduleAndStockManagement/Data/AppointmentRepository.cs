using ScheduleAndStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ScheduleAndStockManagement.Data
{
    public class AppointmentRepository : IGeneralRepository<Appointment>
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.AppointmentItems.ToListAsync();
        }

        public async Task<Appointment> GetByIdAsync(int id)
        {
            return await _context.AppointmentItems.FindAsync(id);
        }

        public async Task AddAsync(Appointment item)
        {
            _context.AppointmentItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appointment item)
        {
            _context.AppointmentItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.AppointmentItems.FindAsync(id);
            if (item is null) return;
            _context.AppointmentItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
