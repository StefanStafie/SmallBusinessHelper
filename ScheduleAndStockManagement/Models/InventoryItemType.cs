using CommunityToolkit.Mvvm.ComponentModel;

namespace ScheduleAndStockManagement.Models
{
    public partial class InventoryItemType : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string name = null!;

        [ObservableProperty]
        private AppointmentType appointmentType = new AppointmentType();

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private bool forSale = false;
        
        public override string ToString()
        {
            return Name;
        }
    }
}
