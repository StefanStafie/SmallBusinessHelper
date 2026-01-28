using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryItemTransactionEditPage : ContentPage
{
    private readonly InventoryItemTransaction inventoryItem;
    private readonly bool isNew;
    private readonly Action<InventoryItemTransaction, bool, bool> saveCallback;

    internal InventoryItemTransactionEditPage(InventoryItemTransaction? item, Action<InventoryItemTransaction, bool, bool> saveCallback)
    {
        InitializeComponent();

        this.saveCallback = saveCallback;
        isNew = item == null;

        inventoryItem = item ?? new InventoryItemTransaction
        {
            InventoryItemType = InventoryPage.InventoryItemsForSale.FirstOrDefault(),
            Quantity = 1,
            UnitPrice = 0,
            AddedAt = DateTime.Now,
            WasSold = false,
        };

        LoadData();
    }

    private void LoadData()
    {
        QtyEntry.Text = inventoryItem.Quantity.ToString();
        UnitPriceEntry.Text = inventoryItem.UnitPrice.ToString();
        DescriptionEntry.Text = inventoryItem.InventoryItemType?.Description;
        this.WasSoldCheckBox.IsChecked = inventoryItem.WasSold;
        AddedAtDatePicker.Date = inventoryItem.AddedAt.Date;
        AddedAtTimePicker.Time = inventoryItem.AddedAt.TimeOfDay;

        InventoryItemTypePicker.ItemsSource = InventoryPage.InventoryItemsForSale;
        if (isNew)
        {
            InventoryItemTypePicker.SelectedItem = InventoryPage.InventoryItemsForSale.FirstOrDefault();
        }
        else
        {
            InventoryItemTypePicker.SelectedItem = inventoryItem;
        }


        AppointmentTypePicker.ItemsSource = InventoryPage.PossibleAppointmentTypes;
        AppointmentTypePicker.SelectedItem = inventoryItem.InventoryItemType?.AppointmentType;
    }

    void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        InventoryItemType selectedItem = (InventoryItemType)picker.SelectedItem;

        if (selectedItem is not null)
        {
            this.AppointmentTypePicker.SelectedItem = selectedItem.AppointmentType;
            this.DescriptionEntry.Text = selectedItem.Description;
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if(InventoryItemTypePicker.SelectedItem == null || AppointmentTypePicker.SelectedItem == null)
        {
            await DisplayAlertAsync("Error", "Please select both a Item Type and an appointment type.", "OK");
            return;
        }

        inventoryItem.InventoryItemType = (InventoryItemType)InventoryItemTypePicker.SelectedItem;
        inventoryItem.Quantity = int.Parse(QtyEntry.Text);
        inventoryItem.UnitPrice = int.Parse(UnitPriceEntry.Text);
        inventoryItem.AddedAt = AddedAtDatePicker.Date.Value.AddTicks(AddedAtTimePicker.Time.Value.Ticks);
        inventoryItem.WasSold = WasSoldCheckBox.IsChecked;

        saveCallback(inventoryItem, isNew, false);
        _ = await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (InventoryItemTypePicker.SelectedItem == null || AppointmentTypePicker.SelectedItem == null)
        {
            _ = await Navigation.PopAsync();
            return;
        }

        saveCallback(inventoryItem, true, true);
        _ = await Navigation.PopAsync();
    }
}
