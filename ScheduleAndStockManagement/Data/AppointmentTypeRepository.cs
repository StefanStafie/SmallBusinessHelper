using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data
{
    public class AppointmentTypeRepository : IGeneralRepository<AppointmentType>
    {
        private readonly AppDbContext _context;

        public AppointmentTypeRepository(AppDbContext context)
        {
            _context = context;
            _ = _context.Database.EnsureCreated();
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
            _ = _context.AppointmentTypes.Add(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppointmentType item)
        {
            _ = _context.AppointmentTypes.Update(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            AppointmentType? item = await _context.AppointmentTypes.FindAsync(id);
            if (item is null)
            {
                return;
            }

            _ = _context.AppointmentTypes.Remove(item);
            _ = await _context.SaveChangesAsync();
        }
    }
}
