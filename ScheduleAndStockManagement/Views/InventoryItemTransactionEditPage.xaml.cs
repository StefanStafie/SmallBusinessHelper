using ScheduleAndStockManagement.Enums;
using ScheduleAndStockManagement.Models;
using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryItemTransactionEditPage : ContentPage
{
    private readonly InventoryItemTransaction inventoryItem;
    private readonly bool isNew;

    public ObservableCollection<InventoryItemType> InventoryItemTypes { get; }

    private readonly Action<InventoryItemTransaction, bool, bool> saveCallback;

    private InventoryTypeDesignation selectedInventoryType;
    public InventoryTypeDesignation SelectedInventoryType
    {
        get => selectedInventoryType;
        set
        {
            selectedInventoryType = value;
            OnPropertyChanged();
            InventoryItemTypePicker.ItemsSource = InventoryItemTypes.Where(item => item.Designation == selectedInventoryType).ToList();
            if(value != InventoryTypeDesignation.ForSaleProducts)
            {
                this.WasBoughtCheckBox.IsChecked = true;
                this.WasSoldCheckBox.IsChecked = false;
                this.WasBoughtCheckBox.IsEnabled = false;
                this.WasSoldCheckBox.IsEnabled = false;
            }
            else
            {
                this.WasBoughtCheckBox.IsEnabled = true;
                this.WasSoldCheckBox.IsEnabled = true;
            }
        }
    }

    internal InventoryItemTransactionEditPage(
        InventoryItemTransaction? item,
        Action<InventoryItemTransaction, bool, bool> saveCallback,
        System.Collections.ObjectModel.ObservableCollection<InventoryItemType> InventoryItemTypes)
    {
        InitializeComponent();

        this.saveCallback = saveCallback;
        this.isNew = item is null;
        this.InventoryItemTypes = InventoryItemTypes;
        inventoryItem = item ?? new InventoryItemTransaction
        {
            InventoryItemType = this.InventoryItemTypes.FirstOrDefault(),
            Description = string.Empty,
            Quantity = 1,
            UnitPrice = 0,
            AddedAt = DateTime.Now,
            WasSold = false,
        };

        LoadData();

        this.BindingContext = this;
    }

    private void LoadData()
    {
        QtyEntry.Text = inventoryItem.Quantity.ToString();
        UnitPriceEntry.Text = inventoryItem.UnitPrice.ToString();
        DescriptionEntry.Text = inventoryItem.Description;
        WasSoldCheckBox.IsChecked = inventoryItem.WasSold;
        WasBoughtCheckBox.IsChecked = !inventoryItem.WasSold;
        AddedAtDatePicker.Date = inventoryItem.AddedAt.Date;
        AddedAtTimePicker.Time = inventoryItem.AddedAt.TimeOfDay;
        SelectedInventoryType = inventoryItem.InventoryItemType.Designation;
        InventoryItemTypePicker.ItemsSource = InventoryItemTypes.Where(item => item.Designation == SelectedInventoryType).ToList();
        
        AppointmentTypePicker.ItemsSource = InventoryItemTransactionsPage.PossibleAppointmentTypes;
        InventoryItemTypePicker.SelectedItem = inventoryItem.InventoryItemType;
        AppointmentTypePicker.SelectedItem = inventoryItem.InventoryItemType?.AppointmentType;
    }

    void OnInventoryItemTypePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        InventoryItemType selectedItem = (InventoryItemType)picker.SelectedItem;

        if (selectedItem is not null)
        {
            this.AppointmentTypePicker.SelectedItem = selectedItem.AppointmentType;
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
        inventoryItem.Description = DescriptionEntry.Text;

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
