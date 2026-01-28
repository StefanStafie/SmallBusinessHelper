using CommunityToolkit.Mvvm.ComponentModel;

namespace ScheduleAndStockManagement.Models
{
    public partial class Inventory : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string name = null!;

        [ObservableProperty]
        private int quantity = 0;

        [ObservableProperty]
        private int unitPrice = 0;

        [ObservableProperty]
        private AppointmentType appointmentType = new AppointmentType();

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private DateTime addedAt = DateTime.Now;

        [ObservableProperty]
        private bool forSale = false;


        public int TotalPrice
        {
            get => Quantity * UnitPrice;
        }
    }
}
