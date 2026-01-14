using ScheduleAndStockManagement.Services;
using System;
using System.Collections.Generic;
using System.Text;

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
