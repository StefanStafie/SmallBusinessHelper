namespace ScheduleAndStockManagement.Models
{
    public class AppointmentType
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public int DurationMinutes { get; set; }
        public int PriceLei { get; set; }

        public string BackgroundColor { get; set; }
    }
}
