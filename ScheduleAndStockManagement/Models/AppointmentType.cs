using CommunityToolkit.Mvvm.ComponentModel;

namespace ScheduleAndStockManagement.Models
{
    public partial class AppointmentType : ObservableObject
    {
        [ObservableProperty]
        private int id = 0;

        [ObservableProperty]
        private string eventName;

        [ObservableProperty] 
        private int durationMinutes;

        [ObservableProperty] 
        private int priceLei;

        [ObservableProperty]
        private uint colorSchemeHex;
        
        public Color TextColor
        {
            get
            {
                return GetContrastingTextColor(Color.FromUint(colorSchemeHex));
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return Color.FromUint(ColorSchemeHex);
            }
        }

        private Color GetContrastingTextColor(Color background)
        {
            double luminance =
                0.299 * background.Red +
                0.587 * background.Green +
                0.114 * background.Blue;

            return luminance > 0.5 ? Colors.Black : Colors.White;
        }

        public string EventNameAndDurationAndPrice
        {
            get => $"{EventName} - {DurationMinutes} min - {PriceLei} lei";
        }

        public override string ToString()
        {
            return EventNameAndDurationAndPrice;
        }
    }
}
