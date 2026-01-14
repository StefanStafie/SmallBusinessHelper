using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public DateTime AddedAt { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
