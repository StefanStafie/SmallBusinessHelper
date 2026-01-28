using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;

public partial class AppointmentTypeEditPage : ContentPage
{
    private readonly AppointmentType _item;
    private readonly bool _isNew;
    private readonly Action<AppointmentType, bool, bool> _saveCallback;

    internal AppointmentTypeEditPage(AppointmentType? item, Action<AppointmentType, bool, bool> saveCallback)
    {
        InitializeComponent();

        _saveCallback = saveCallback;
        _isNew = item == null;

        _item = item ?? new AppointmentType
        {
            Id = AppointmentTypePage.NextAppointmentId,
            EventName = string.Empty,
            ColorSchemeHex = 0xFF00FFFF,
            DurationMinutes = 60
        }; 
        

        LoadData();
    }

    private void LoadData()
    {
        NameEntry.Text = _item.EventName;
        Minutes.Text = _item.DurationMinutes.ToString();
        Price.Text = _item.PriceLei.ToString();
        colorPicker.SelectedColor = Color.FromUint(_item.ColorSchemeHex);
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _item.EventName = NameEntry.Text;
        _item.DurationMinutes = (int)float.Parse(Minutes.Text);
        _item.PriceLei = (int)float.Parse(Price.Text);
       _item.ColorSchemeHex = colorPicker.SelectedColor.ToUint();

        _saveCallback(_item, _isNew, false);
        _ = await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        _saveCallback(_item, true, true);
        _ = await Navigation.PopAsync();
    }
}
