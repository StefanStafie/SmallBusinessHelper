using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int AppointmentTypeId { get; set; }
        public int CustomerId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool IsAllDay { get; set; }
        public string EventName { get; set; }
        public string Notes { get; set; }
        public string BackgroundColor { get; set; }
    }
}
