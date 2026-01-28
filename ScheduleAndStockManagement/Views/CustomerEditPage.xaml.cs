using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;

public partial class CustomerEditPage : ContentPage
{
    private readonly Customer _item;
    private readonly bool _isNew;
    private readonly Action<Customer, bool, bool> _saveCallback;

    internal CustomerEditPage(Customer? item, Action<Customer, bool, bool> saveCallback)
    {
        InitializeComponent();

        _saveCallback = saveCallback;
        _isNew = item == null;

        _item = item ?? new Customer
        {
            Id = CustomerPage.NextCustomerId,
            Name = string.Empty,
            Phone = string.Empty,
            AddedAt = DateTime.Now
        };

        LoadData();
    }

    private void LoadData()
    {
        NameEntry.Text = _item.Name;
        PhoneEntry.Text = _item.Phone;

        AddedAtDatePicker.Date = _item.AddedAt.Date;
        AddedAtTimePicker.Time = _item.AddedAt.TimeOfDay;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _item.Name = NameEntry.Text;
        _item.Phone = PhoneEntry.Text;
        _item.AddedAt = AddedAtDatePicker.Date.Value
                            .AddTicks(AddedAtTimePicker.Time.Value.Ticks);

        _saveCallback(_item, _isNew, false);
        _ = await Navigation.PopAsync();
    }
}
