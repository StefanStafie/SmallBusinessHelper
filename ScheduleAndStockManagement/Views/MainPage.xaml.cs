using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Services;
using ScheduleAndStockManagement.ViewModels;
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
            await Navigation.PushAsync(new SchedulingAssistant());
        }

        private async void OnStockClicked(object? sender, EventArgs e)
        {
            var inventoryPage = App.Services.GetService<InventoryPage>();
            await Navigation.PushAsync(inventoryPage);
        }
        private async void OnAppointmentTypesClicked(object? sender, EventArgs e)
        {
            var appointmentPage = App.Services.GetService<AppointmentTypePage>();
            await Navigation.PushAsync(appointmentPage);
        }

        private async void OnCustomerClicked(object? sender, EventArgs e)
        {
            var customerPage = App.Services.GetService<CustomerPage>();
            await Navigation.PushAsync(customerPage);
        }
    }
}
