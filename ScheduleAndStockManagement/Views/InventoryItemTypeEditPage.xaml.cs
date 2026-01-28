using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryItemTypeEditPage : ContentPage
{
    private readonly InventoryItemType inventoryItemType;
    private readonly bool isNew;
    private readonly Action<InventoryItemType, bool, bool> saveCallback;

    internal InventoryItemTypeEditPage(InventoryItemType? item, Action<InventoryItemType, bool, bool> saveCallback)
    {
        InitializeComponent();

        this.saveCallback = saveCallback;
        isNew = item == null;

        inventoryItemType = item ?? new InventoryItemType
        {
            Name = string.Empty,
            AppointmentType = InventoryPage.PossibleAppointmentTypes.First(),
            Description = string.Empty,
            ForSale = false,
        };

        LoadData();
    }

    private void LoadData()
    {
        NameEntry.Text = inventoryItemType.Name;
        DescriptionEntry.Text = inventoryItemType.Description;
        ForSaleCheckbox.IsChecked = inventoryItemType.ForSale;
        AppointmentTypePicker.ItemsSource = InventoryPage.PossibleAppointmentTypes;
        AppointmentTypePicker.SelectedItem = inventoryItemType.AppointmentType;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        inventoryItemType.Name = NameEntry.Text;
        inventoryItemType.Description = DescriptionEntry.Text;
        inventoryItemType.AppointmentType = (AppointmentType)AppointmentTypePicker.SelectedItem;
        inventoryItemType.ForSale = ForSaleCheckbox.IsChecked;

        this.saveCallback(inventoryItemType, isNew, false);
        _ = await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        saveCallback(inventoryItemType, true, true);
        _ = await Navigation.PopAsync();
    }
}
