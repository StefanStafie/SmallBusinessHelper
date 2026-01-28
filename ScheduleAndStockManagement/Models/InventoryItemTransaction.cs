using CommunityToolkit.Mvvm.ComponentModel;
using ScheduleAndStockManagement.Intefaces;

namespace ScheduleAndStockManagement.Models
{
    public partial class InventoryItemTransaction : ObservableObject, IMoneySpender, IMoneyMaker
    {
        public int Id { get; set; }

        [ObservableProperty]
        private InventoryItemType  inventoryItemType = null;

        [ObservableProperty]
        private int quantity = 0;

        [ObservableProperty]
        private int unitPrice = 0;

        [ObservableProperty]
        private DateTime addedAt = DateTime.Now;
        
        [ObservableProperty]
        private bool wasSold = false;
        
        public int TotalPrice
        {
            get => Quantity * UnitPrice;
        }
        public override string ToString()
        {
            return InventoryItemType is not null? InventoryItemType.Name : string.Empty;
        }

        public int CalculateSpendings()
        {
            if (WasSold)
            {
                return 0;
            }

            return Quantity * UnitPrice;
        }

        public DateTime SpendingDateTime()
        {
            return AddedAt;
        }

        public int CalculateEarnings()
        {
            if (WasSold)
            {
                return Quantity * UnitPrice;
            }

            return 0;
        }

        public DateTime EarningDateTime()
        {
            return AddedAt;
        }
    }
}
