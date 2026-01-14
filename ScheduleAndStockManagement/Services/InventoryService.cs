using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Services
{
    public class InventoryService
    {
        private readonly IGeneralRepository<Inventory> _repo;

        public InventoryService(IGeneralRepository<Inventory> repo)
        {
            _repo = repo;
        }

        public Task<List<Inventory>> GetItemsAsync() => _repo.GetAllAsync();
        public Task AddItemAsync(Inventory item) => _repo.AddAsync(item);
        public Task UpdateItemAsync(Inventory item) => _repo.UpdateAsync(item);
        public Task DeleteItemAsync(int id) => _repo.DeleteAsync(id);
    }

}
