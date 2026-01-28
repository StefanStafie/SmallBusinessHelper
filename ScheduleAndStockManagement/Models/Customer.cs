using CommunityToolkit.Mvvm.ComponentModel;

namespace ScheduleAndStockManagement.Models
{
    public partial class Customer : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        public DateTime addedAt = DateTime.Now;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        public string phone = string.Empty;

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
