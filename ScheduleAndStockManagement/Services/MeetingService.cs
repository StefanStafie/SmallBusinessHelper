using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Services
{
    public class MeetingService
    {
        private readonly IGeneralRepository<Meeting> _repo;

        public MeetingService(IGeneralRepository<Meeting> repo)
        {
            _repo = repo;
        }

        public Task<List<Meeting>> GetItemsAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task AddItemAsync(Meeting item)
        {
            return _repo.AddAsync(item);
        }

        public Task UpdateItemAsync(Meeting item)
        {
            return _repo.UpdateAsync(item);
        }

        public Task DeleteItemAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }
}
