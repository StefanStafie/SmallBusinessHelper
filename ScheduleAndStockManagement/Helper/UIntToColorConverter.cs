using System.Globalization;
using Microsoft.Maui.Graphics;

namespace ScheduleAndStockManagement.Helper
{
    public class UIntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is uint argb)
                return Color.FromUint(argb);

            return Colors.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
                return color.ToUint();

            return 0u;
        }
    }
}
