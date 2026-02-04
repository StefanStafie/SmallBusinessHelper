using Microsoft.EntityFrameworkCore;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Data
{
    public class MeetingRepository : IGeneralRepository<Meeting>
    {
        private readonly AppDbContext _context;

        public MeetingRepository(AppDbContext context)
        {
            _context = context;
            _ = _context.Database.EnsureCreated();
        }

        public async Task<List<Meeting>> GetAllAsync()
        {
            var meetings = await _context.Meetings.ToListAsync();

            foreach (var meeting in meetings)
            {
                meeting.Customer.CustomerFiles = await _context.CustomerFiles
                    .Where(f => f.CustomerId == meeting.Customer.Id)
                    .ToListAsync();
            }

            return meetings;
        }

        public async Task<Meeting> GetByIdAsync(int id)
        {
            return await _context.Meetings.FindAsync(id);
        }

        public async Task AddAsync(Meeting item)
        {
            _ = _context.Meetings.Add(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Meeting item)
        {
            _ = _context.Meetings.Update(item);
            _ = await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Meeting? item = await _context.Meetings.FindAsync(id);
            if (item is null)
            {
                return;
            }

            _ = _context.Meetings.Remove(item);
            _ = await _context.SaveChangesAsync();
        }
    }
}
