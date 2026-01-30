using ScheduleAndStockManagement.Views;

namespace ScheduleAndStockManagement
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnScheduleClicked(object? sender, EventArgs e)
        {
            SchedulingAssistantPage? schedulingAssistant = App.Services.GetService<SchedulingAssistantPage>();
            await Navigation.PushAsync(schedulingAssistant);
        }

        private async void OnStockClicked(object? sender, EventArgs e)
        {
            InventoryItemTypesPage? inventoryPage = App.Services.GetService<InventoryItemTypesPage>();
            await Navigation.PushAsync(inventoryPage);
        }
        private async void OnAppointmentTypesClicked(object? sender, EventArgs e)
        {
            AppointmentTypePage? appointmentPage = App.Services.GetService<AppointmentTypePage>();
            await Navigation.PushAsync(appointmentPage);
        }

        private async void OnCustomerClicked(object? sender, EventArgs e)
        {
            CustomerPage? customerPage = App.Services.GetService<CustomerPage>();
            await Navigation.PushAsync(customerPage);
        }

        private async void OnInventoryItemTransactionsButtonClicked(object? sender, EventArgs e)
        {
            InventoryItemTransactionsPage? transactionsPage = App.Services.GetService<InventoryItemTransactionsPage>();
            await Navigation.PushAsync(transactionsPage);
        }

        private async void OnGraphsAndReportsButtonClicked(object? sender, EventArgs e)
        {
            ChartSelectionPage? chartSelectionPage = App.Services.GetService<ChartSelectionPage>();
            await Navigation.PushAsync(chartSelectionPage);
        }
    }
}
