using ScheduleAndStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace ScheduleAndStockManagement.Data
{
    public class AppointmentTypeRepository : IGeneralRepository<AppointmentType>
    {
        private readonly AppDbContext _context;

        public AppointmentTypeRepository(AppDbContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public async Task<List<AppointmentType>> GetAllAsync()
        {
            return await _context.AppointmentTypes.ToListAsync();
        }

        public async Task<AppointmentType> GetByIdAsync(int id)
        {
            return await _context.AppointmentTypes.FindAsync(id);
        }

        public async Task AddAsync(AppointmentType item)
        {
            _context.AppointmentTypes.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppointmentType item)
        {
            _context.AppointmentTypes.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var item = await _context.AppointmentTypes.FindAsync(id);
            if (item is null) return;
            _context.AppointmentTypes.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}
