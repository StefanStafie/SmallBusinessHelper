using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;

public partial class AppointmentTypeEditPage : ContentPage
{
    private AppointmentType _item;
    private bool _isNew;
    private Action<AppointmentType, bool, bool> _saveCallback;

    internal AppointmentTypeEditPage(AppointmentType? item, Action<AppointmentType, bool, bool> saveCallback)
    {
        InitializeComponent();

        _saveCallback = saveCallback;
        _isNew = item == null;

        _item = item ?? new AppointmentType
        {
            Id = AppointmentTypePage.NextAppointmentId,
            EventName = string.Empty,
            BackgroundColor = "Red",
            DurationMinutes = 60
        };

        LoadData();
    }

    private void LoadData()
    {
        NameEntry.Text = _item.EventName;
        Minutes.Text = (_item.DurationMinutes).ToString();
        Price.Text = _item.PriceLei.ToString();
        BackgroundColorPicker.ItemsSource = AppointmentTypePage.PossibleBackgroundColors;
        BackgroundColorPicker.SelectedItem = _item.BackgroundColor;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _item.EventName = NameEntry.Text;
        _item.DurationMinutes = (int)float.Parse(Minutes.Text);
        _item.PriceLei = (int)float.Parse(Price.Text);
        _item.BackgroundColor = (string)BackgroundColorPicker.SelectedItem;

        _saveCallback(_item, _isNew, false);
        await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Delete", "Are you sure?", "Yes", "No");
        if (!confirm) return;

        _saveCallback(_item, true, true);
        await Navigation.PopAsync();
    }
}
