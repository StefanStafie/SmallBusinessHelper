using ScheduleAndStockManagement.Enums;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryItemTransactionsPage : ContentPage
{
    public ObservableCollection<InventoryItemType> InventoryItemTypes { get; set; }
    public ObservableCollection<InventoryItemTransaction> InventoryItemTransactions { get; set; }
    private readonly InventoryItemTypeService inventoryItemTypeService;
    private readonly InventoryItemTransactionService inventoryItemTransactionService;
    private readonly AppointmentTypeService appointmentTypeService;
    public static List<AppointmentType> PossibleAppointmentTypes { get; private set; }
    
    private InventoryTypeDesignation selectedInventoryType;
    public InventoryTypeDesignation SelectedInventoryType
    {
        get => selectedInventoryType;
        set 
        { 
            selectedInventoryType = value; 
            OnPropertyChanged();
            InventoryList.ItemsSource = InventoryItemTransactions.Where(item => item.InventoryItemType.Designation == selectedInventoryType).ToList();
        }
    }

    public InventoryItemTransactionsPage(InventoryItemTypeService inventoryItemTypeService, InventoryItemTransactionService inventoryItemTransactionService, AppointmentTypeService appointmentTypeService)
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

        InventoryList.ItemsSource = InventoryItemTransactions;

        SelectedInventoryType = InventoryTypeDesignation.ForSaleProducts;
        BindingContext = this;
    }

    private async void OnAddItemTransactionClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryItemTransactionEditPage(null, SaveItemTransaction, InventoryItemTypes));
    }

    private async void OnItemClicked(object sender, EventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is InventoryItemTransaction selected)
        {
            await Navigation.PushAsync(new InventoryItemTransactionEditPage(selected, SaveItemTransaction, InventoryItemTypes));
        }
    }

    private void SaveItemTransaction(InventoryItemTransaction inventoryItemTransaction, bool isNew, bool isDeleted)
    {
        if (isDeleted)
        {
            InventoryItemTransaction existing = InventoryItemTransactions.First(x => x.Id == inventoryItemTransaction.Id);
            _ = InventoryItemTransactions.Remove(existing);
            _ = inventoryItemTypeService.DeleteItemAsync(inventoryItemTransaction.Id);
            return;
        }

        if (isNew)
        {
            InventoryItemTransactions.Add(inventoryItemTransaction);
            _ = inventoryItemTransactionService.AddItemAsync(inventoryItemTransaction);
        }
        else
        {
            InventoryItemTransaction existing = InventoryItemTransactions.First(x => x.Id == inventoryItemTransaction.Id);
            int index = InventoryItemTransactions.IndexOf(existing);
            InventoryItemTransactions[index] = inventoryItemTransaction;
            _ = inventoryItemTransactionService.UpdateItemAsync(inventoryItemTransaction);
        }
    }
    private void OnFilterChanged(object sender, TextChangedEventArgs e)
    {
        string wasSoldFilter = FilterWasSold.Text?.ToLower() ?? "";
        string nameFilter = FilterName.Text?.ToLower() ?? "";
        string quantityFilter = FilterQuantity.Text?.ToLower() ?? "";
        string unitPriceFilter = FilterUnitPrice.Text?.ToLower() ?? "";
        string dateFilter = FilterDate.Text?.ToLower() ?? "";

        var filtered = InventoryItemTransactions.Where(item =>

            (string.IsNullOrWhiteSpace(wasSoldFilter) ||
                (wasSoldFilter.Contains("sold") && item.WasSold) ||
                (wasSoldFilter.Contains("not") && !item.WasSold)) &&

            (string.IsNullOrWhiteSpace(nameFilter) ||
                (item.InventoryItemType?.ToString()?.ToLower().Contains(nameFilter) ?? false)) &&

            (string.IsNullOrWhiteSpace(quantityFilter) ||
                item.Quantity.ToString().ToLower().Contains(quantityFilter)) &&

            (string.IsNullOrWhiteSpace(unitPriceFilter) ||
                item.UnitPrice.ToString().ToLower().Contains(unitPriceFilter)) &&

            (string.IsNullOrWhiteSpace(dateFilter) ||
                item.AddedAt.ToString("yyyy-MM-dd").Contains(dateFilter))
        ).ToList();

        InventoryList.ItemsSource = filtered;
    }


}
