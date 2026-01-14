using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryEditPage : ContentPage
{
    private Inventory _item;
    private bool _isNew;
    private Action<Inventory, bool, bool> _saveCallback;
    private AppointmentType types;

    internal InventoryEditPage(Inventory? item, Action<Inventory, bool, bool> saveCallback)
    {
        InitializeComponent();

        _saveCallback = saveCallback;
        _isNew = item == null;

        _item = item ?? new Inventory
        {
            Id = InventoryPage.NextInventoryId,
            Name = string.Empty,
            AddedAt = DateTime.Now,
            AppointmentType = InventoryPage.PossibleAppointmentTypes.First()
        };

        LoadData();
    }

    private void LoadData()
    {
        NameEntry.Text = _item.Name;
        QtyEntry.Text = _item.Quantity.ToString();
        UnitPriceEntry.Text = _item.UnitPrice.ToString();
        DescriptionEntry.Text = _item.Description;
        AppointmentTypePicker.ItemsSource = InventoryPage.PossibleAppointmentTypes.Select(x=>x.EventName).ToList();
        AddedAtDatePicker.Date = _item.AddedAt.Date;
        AddedAtTimePicker.Time = _item.AddedAt.TimeOfDay;

        AppointmentTypePicker.SelectedItem = _item.AppointmentType.EventName;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _item.Name = NameEntry.Text;
        _item.Quantity = int.Parse(QtyEntry.Text);
        _item.UnitPrice = int.Parse(UnitPriceEntry.Text);
        _item.TotalPrice = _item.Quantity * _item.UnitPrice;
        _item.Description = DescriptionEntry.Text;
        _item.AppointmentType = InventoryPage.PossibleAppointmentTypes.Find(x=>x.EventName == (string)AppointmentTypePicker.SelectedItem);
        _item.AddedAt = AddedAtDatePicker.Date.Value.AddTicks(AddedAtTimePicker.Time.Value.Ticks);

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
