namespace ScheduleAndStockManagement.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}
