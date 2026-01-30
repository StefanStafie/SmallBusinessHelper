using CommunityToolkit.Mvvm.ComponentModel;

namespace ScheduleAndStockManagement.Models
{
    public class MoneyDatePoint
    {
        public DateTime Date { get; set; }
        public int Earnings { get; set; }
        public int Spendings { get; set; }
    }
}
