using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Services
{
    public class AppointmentTypeService
    {
        private readonly IGeneralRepository<AppointmentType> _repo;

        public AppointmentTypeService(IGeneralRepository<AppointmentType> repo)
        {
            _repo = repo;
        }

        public Task<List<AppointmentType>> GetItemsAsync() => _repo.GetAllAsync();
        public Task AddItemAsync(AppointmentType item) => _repo.AddAsync(item);
        public Task UpdateItemAsync(AppointmentType item) => _repo.UpdateAsync(item);
        public Task DeleteItemAsync(int id) => _repo.DeleteAsync(id);
    }

}
