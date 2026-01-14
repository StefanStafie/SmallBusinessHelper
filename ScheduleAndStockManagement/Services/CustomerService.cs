using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Services
{
    public class CustomerService
    {
        private readonly IGeneralRepository<Customer> _repo;

        public CustomerService(IGeneralRepository<Customer> repo)
        {
            _repo = repo;
        }

        public Task<List<Customer>> GetItemsAsync() => _repo.GetAllAsync();
        public Task AddItemAsync(Customer item) => _repo.AddAsync(item);
        public Task UpdateItemAsync(Customer item) => _repo.UpdateAsync(item);
        public Task DeleteItemAsync(int id) => _repo.DeleteAsync(id);
    }

}
