using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryPage : ContentPage
{
    public ObservableCollection<InventoryItemType> InventoryItemTypes { get; set; }
    public ObservableCollection<InventoryItemTransaction> InventoryItemTransactions { get; set; }
    private readonly InventoryItemTypeService inventoryItemTypeService;
    private readonly InventoryItemTransactionService inventoryItemTransactionService;
    private readonly AppointmentTypeService appointmentTypeService;
    public static List<AppointmentType> PossibleAppointmentTypes { get; private set; }

    public static List<InventoryItemType> InventoryItemsForSale { get; private set; }

    public InventoryPage(InventoryItemTypeService inventoryItemTypeService, InventoryItemTransactionService inventoryItemTransactionService, AppointmentTypeService appointmentTypeService)
    {
        InitializeComponent();
        this.inventoryItemTypeService = inventoryItemTypeService;
        this.inventoryItemTransactionService = inventoryItemTransactionService;
        this.appointmentTypeService = appointmentTypeService;

        PossibleAppointmentTypes = this.appointmentTypeService.GetItemsAsync().Result;
        InventoryItemTypes = new ObservableCollection<InventoryItemType>(
            inventoryItemTypeService.GetItemsAsync().Result
        ); 
        this.InventoryItemTransactions = new ObservableCollection<InventoryItemTransaction>(
            inventoryItemTransactionService.GetItemsAsync().Result
        );
        InventoryList.ItemsSource = InventoryItemTypes;
    }

    private async void OnItemTransactionClicked(object sender, EventArgs e)
    {
        this.RefreshItemsForSale();
        await Navigation.PushAsync(new InventoryItemTransactionEditPage(null, SaveItemTransaction));
    }

    private async void OnAddItemTypeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryItemTypeEditPage(null, SaveItemType));
    }

    private void RefreshItemsForSale()
    {
        var itemsForSale = InventoryItemTypes.Where(x=>x.ForSale == true);
        InventoryItemsForSale = itemsForSale.ToList();
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
}
