using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;

public partial class AppointmentTypeEditPage : ContentPage
{
    private readonly AppointmentType appointmentTypeItem;
    private readonly bool isNew;
    private readonly Action<AppointmentType, bool, bool> _saveCallback;

    internal AppointmentTypeEditPage(AppointmentType? item, Action<AppointmentType, bool, bool> saveCallback)
    {
        InitializeComponent();

        _saveCallback = saveCallback;
        isNew = item == null;

        this.appointmentTypeItem = item ?? new AppointmentType
        {
            EventName = string.Empty,
            ColorSchemeHex = 0xFF00FFFF,
            DurationMinutes = 60
        }; 

        LoadData();
    }

    private void LoadData()
    {
        NameEntry.Text = appointmentTypeItem.EventName;
        Minutes.Text = appointmentTypeItem.DurationMinutes.ToString();
        Price.Text = appointmentTypeItem.PriceLei.ToString();
        colorPicker.SelectedColor = Color.FromUint(appointmentTypeItem.ColorSchemeHex);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        appointmentTypeItem.EventName = NameEntry.Text;
        appointmentTypeItem.DurationMinutes = (int)float.Parse(Minutes.Text);
        appointmentTypeItem.PriceLei = (int)float.Parse(Price.Text);
       appointmentTypeItem.ColorSchemeHex = colorPicker.SelectedColor.ToUint();

        _saveCallback(appointmentTypeItem, isNew, false);
        _ = await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        _saveCallback(appointmentTypeItem, true, true);
        _ = await Navigation.PopAsync();
    }
}
