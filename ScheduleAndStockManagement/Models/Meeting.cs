using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScheduleAndStockManagement.Intefaces;

namespace ScheduleAndStockManagement.Models
{
    public partial class Meeting : ObservableObject, IMoneyMaker
    {
        [ObservableProperty]
        private int id = 0;

        [ObservableProperty]
        public AppointmentType appointmentType = null;

        [ObservableProperty]
        public Customer customer = null;

        [ObservableProperty]
        public DateTime from;

        [ObservableProperty]
        public DateTime to;

        [ObservableProperty]
        public string notes = string.Empty;

        [ObservableProperty]
        public int tipAmount = 0;

        [ObservableProperty]
        public bool isAllDay = false;

        [ObservableProperty]
        public int priceLei = 0;

        partial void OnAppointmentTypeChanged(AppointmentType newValue)
        {
            PriceLei = newValue.PriceLei;
            to = from.AddMinutes(newValue.DurationMinutes);
        }

        public string EventNameAndCustomer => this.Customer != null && this.AppointmentType != null
            ? $"{this.Customer.Name} - {this.AppointmentType.EventName}"
            : string.Empty;

        public Color BackgroundColor => AppointmentType != null
            ? AppointmentType.BackgroundColor
            : Colors.Gray;
        public Color TextColor => AppointmentType != null
            ? AppointmentType.TextColor
            : Colors.Black;

        public Brush BackgroundColorBrush => new SolidColorBrush(BackgroundColor);


        public override string ToString()
        {
            return $"{EventNameAndCustomer}";
        }

        public int CalculateEarnings()
        {
            return PriceLei + TipAmount;
        }

        public DateTime EarningDateTime()
        {
            return From;
        }
    }
}