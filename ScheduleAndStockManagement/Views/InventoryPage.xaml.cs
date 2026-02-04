using ScheduleAndStockManagement.Enums;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryItemTypesPage : ContentPage
{
    public ObservableCollection<InventoryItemType> InventoryItemTypes { get; set; }
    public ObservableCollection<InventoryItemTransaction> InventoryItemTransactions { get; set; }
    private readonly InventoryItemTypeService inventoryItemTypeService;
    private readonly InventoryItemTransactionService inventoryItemTransactionService;
    private readonly AppointmentTypeService appointmentTypeService;
    public static List<AppointmentType> PossibleAppointmentTypes { get; private set; }

    public static List<InventoryItemType> InventoryItemsForSale { get; private set; }

    private InventoryTypeDesignation selectedInventoryType;
    public InventoryTypeDesignation SelectedInventoryType
    {
        get => selectedInventoryType;
        set
        {
            selectedInventoryType = value;
            OnPropertyChanged();
            InventoryList.ItemsSource = InventoryItemTypes.Where(item => item.Designation == selectedInventoryType).ToList();
        }
    }
    public InventoryItemTypesPage(InventoryItemTypeService inventoryItemTypeService, InventoryItemTransactionService inventoryItemTransactionService, AppointmentTypeService appointmentTypeService)
    {
        InitializeComponent();
        this.inventoryItemTypeService = inventoryItemTypeService;
        this.inventoryItemTransactionService = inventoryItemTransactionService;
        this.appointmentTypeService = appointmentTypeService;

        PossibleAppointmentTypes = this.appointmentTypeService.GetItemsAsync().Result;
        this.InventoryItemTypes = new ObservableCollection<InventoryItemType>(
            inventoryItemTypeService.GetItemsAsync().Result
        ); 
        this.InventoryItemTransactions = new ObservableCollection<InventoryItemTransaction>(
            inventoryItemTransactionService.GetItemsAsync().Result
        );

        BindingContext = this;
        InventoryList.ItemsSource = InventoryItemTypes;
        SelectedInventoryType = InventoryTypeDesignation.ForSaleProducts;
    }


    private async void OnAddItemTypeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryItemTypeEditPage(null, SaveItemType));
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is InventoryItemType selected)
        {
            await Navigation.PushAsync(new InventoryItemTypeEditPage(selected, SaveItemType));
        }
    }

    private void SaveItemType(InventoryItemType inventoryItemType, bool isNew, bool isDeleted)
    {
        if (isDeleted)
        {
            InventoryItemType existing = InventoryItemTypes.First(x => x.Id == inventoryItemType.Id);
            _ = InventoryItemTypes.Remove(existing);
            _ = inventoryItemTypeService.DeleteItemAsync(inventoryItemType.Id);
            return;
        }

        if (isNew)
        {
            InventoryItemTypes.Add(inventoryItemType);
            _ = inventoryItemTypeService.AddItemAsync(inventoryItemType);
        }
        else
        {
            InventoryItemType existing = InventoryItemTypes.First(x => x.Id == inventoryItemType.Id);
            int index = InventoryItemTypes.IndexOf(existing);
            InventoryItemTypes[index] = inventoryItemType;
            _ = inventoryItemTypeService.UpdateItemAsync(inventoryItemType);
        }
    }

    private void OnFilterChanged(object sender, TextChangedEventArgs e)
    {
        string idFilter = FilterId.Text?.ToLower() ?? "";
        string designationFilter = FilterDesignation.Text?.ToLower() ?? "";
        string nameFilter = FilterName.Text?.ToLower() ?? "";
        string appointmentFilter = FilterAppointmentType.Text?.ToLower() ?? "";

        var filtered = this.InventoryItemTypes.Where(item =>
            item.Id.ToString().ToLower().Contains(idFilter) &&
            (item.Designation.ToString().ToLower().Contains(designationFilter) == true) &&
            (item.Name?.ToLower().Contains(nameFilter) ?? true) &&
            (item.AppointmentType?.EventName?.ToLower().Contains(appointmentFilter) ?? true)
        ).ToList();

        InventoryList.ItemsSource = filtered;
    }
}
