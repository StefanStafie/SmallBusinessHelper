using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryPage : ContentPage
{
    public ObservableCollection<Inventory> Items { get; set; }

    private readonly InventoryService _inventoryService;
    private readonly AppointmentTypeService _appointmentTypeService;
    public static List<AppointmentType>? PossibleAppointmentTypes { get; private set; }
    private static readonly object inventoryLock = new();

    public static int NextInventoryId
    {
        get
        {
            lock (inventoryLock)
            {
                field++;
                return field;
            }
        }
        set
        {
            field = value;
        }
    } = 0;

    public InventoryPage(InventoryService inventoryService, AppointmentTypeService appointmentTypeService)
    {
        InitializeComponent();
        _inventoryService = inventoryService;
        _appointmentTypeService = appointmentTypeService;

        PossibleAppointmentTypes = _appointmentTypeService.GetItemsAsync().Result;
        Items = new ObservableCollection<Inventory>(
            _inventoryService.GetItemsAsync().Result
        );

        InventoryList.ItemsSource = Items;
        NextInventoryId = Items.Count > 0 ? Items.Max(i => i.Id) : 0;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryEditPage(null, SaveItem));
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Inventory selected)
        {
            await Navigation.PushAsync(new InventoryEditPage(selected, SaveItem));
        }
    }

    private void SaveItem(Inventory item, bool isNew, bool isDeleted)
    {
        if (isDeleted)
        {
            Inventory existing = Items.First(x => x.Id == item.Id);
            _ = Items.Remove(existing);
            _ = _inventoryService.DeleteItemAsync(item.Id);
            return;
        }

        if (isNew)
        {
            Items.Add(item);
            _ = _inventoryService.AddItemAsync(item);
        }
        else
        {
            Inventory existing = Items.First(x => x.Id == item.Id);
            int index = Items.IndexOf(existing);
            Items[index] = item;
            _ = _inventoryService.UpdateItemAsync(item);
        }
    }
}
