using ScheduleAndStockManagement.Services;

namespace ScheduleAndStockManagement.ViewModels
{
    internal class InventoryViewModel
    {
        private readonly InventoryService _service;

        public InventoryViewModel(InventoryService service)
        {
            _service = service;
        }
    }
}
