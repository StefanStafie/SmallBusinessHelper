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
            SchedulingAssistant? schedulingAssistant = App.Services.GetService<SchedulingAssistant>();
            await Navigation.PushAsync(schedulingAssistant);
        }

        private async void OnStockClicked(object? sender, EventArgs e)
        {
            InventoryPage? inventoryPage = App.Services.GetService<InventoryPage>();
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
    }
}
