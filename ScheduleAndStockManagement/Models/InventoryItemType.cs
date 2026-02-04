using CommunityToolkit.Mvvm.ComponentModel;
using ScheduleAndStockManagement.Enums;

namespace ScheduleAndStockManagement.Models
{
    public partial class InventoryItemType : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private AppointmentType appointmentType = new AppointmentType();


        [ObservableProperty]
        private InventoryTypeDesignation designation;
        
        public override string ToString()
        {
            return Name;
        }
    }
}
