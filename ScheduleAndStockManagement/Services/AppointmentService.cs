using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Services
{
    public class AppointmentService
    {
        private readonly IGeneralRepository<Appointment> _repo;

        public AppointmentService(IGeneralRepository<Appointment> repo)
        {
            _repo = repo;
        }

        public Task<List<Appointment>> GetItemsAsync() => _repo.GetAllAsync();
        public Task AddItemAsync(Appointment item) => _repo.AddAsync(item);
        public Task UpdateItemAsync(Appointment item) => _repo.UpdateAsync(item);
        public Task DeleteItemAsync(int id) => _repo.DeleteAsync(id);
    }

}
