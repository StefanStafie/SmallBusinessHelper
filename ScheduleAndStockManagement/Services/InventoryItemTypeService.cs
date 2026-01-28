using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Services
{
    public class InventoryItemTypeService
    {
        private readonly IGeneralRepository<InventoryItemType> _repo;

        public InventoryItemTypeService(IGeneralRepository<InventoryItemType> repo)
        {
            _repo = repo;
        }

        public Task<List<InventoryItemType>> GetItemsAsync()
        {
            return _repo.GetAllAsync();
        }

        public Task AddItemAsync(InventoryItemType item)
        {
            return _repo.AddAsync(item);
        }

        public Task UpdateItemAsync(InventoryItemType item)
        {
            return _repo.UpdateAsync(item);
        }

        public Task DeleteItemAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }

}
