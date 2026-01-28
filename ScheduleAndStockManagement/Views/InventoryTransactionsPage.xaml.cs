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

    public static List<InventoryItemType> InventoryItemsForSale { get; private set; }

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

        BindingContext = this;
        InventoryList.ItemsSource = InventoryItemTransactions;
    }

    private async void OnAddItemTransactionClicked(object sender, EventArgs e)
    {
        InventoryItemsForSale = InventoryItemTypes.Where(itemType => itemType.ForSale).ToList();
        await Navigation.PushAsync(new InventoryItemTransactionEditPage(null, SaveItemTransaction));
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is InventoryItemTransaction selected)
        {
            InventoryItemsForSale = InventoryItemTypes.Where(itemType => itemType.ForSale).ToList();
            await Navigation.PushAsync(new InventoryItemTransactionEditPage(selected, SaveItemTransaction));
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
