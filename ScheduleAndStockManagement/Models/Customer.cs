using CommunityToolkit.Mvvm.ComponentModel;

namespace ScheduleAndStockManagement.Models
{
    public partial class Customer : ObservableObject
    {
        [ObservableProperty]
        public int id;

        [ObservableProperty]
        public DateTime addedAt = DateTime.Now;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        public string phone = string.Empty;

        [ObservableProperty]
        public List<CustomerFile> customerFiles = new List<CustomerFile>();
        
        public string NameAndPhone
        {
            get => $"{Name} ({Phone})";
        }

        public override string ToString()
        {
            return NameAndPhone;
        }
    }
}
