using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Services
{
    public class InventoryItemTransactionService
    {
        private readonly IGeneralRepository<InventoryItemTransaction> _repo;

        public InventoryItemTransactionService(IGeneralRepository<InventoryItemTransaction> repo)
        {
            _repo = repo;
        }

        public Task<List<InventoryItemTransaction>> GetItemsAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task AddItemAsync(InventoryItemTransaction item)
        {
            return _repo.AddAsync(item);
        }

        public Task UpdateItemAsync(InventoryItemTransaction item)
        {
            return _repo.UpdateAsync(item);
        }

        public Task DeleteItemAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }

}
