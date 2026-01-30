using Syncfusion.Maui.Charts;

namespace ScheduleAndStockManagement.Views;

public partial class ChartDisplayPage : ContentPage
{
    public ChartDisplayPage(SfCartesianChart chart)
    {
        InitializeComponent();
        this.Content = chart;
    }
}

