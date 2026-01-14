using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryPage : ContentPage
{
    internal ObservableCollection<Inventory> Items { get; set; } = new ObservableCollection<Inventory>();

    private readonly InventoryService _inventoryService;
    private readonly AppointmentTypeService _appointmentTypeService;
    public static List<AppointmentType> PossibleAppointmentTypes { get; private set; }
    private static object inventoryLock = new object();
    private static int _lastInventoryId = 0;
    public static int NextInventoryId
    {
        get
        {
            lock (inventoryLock)
            {
                _lastInventoryId++;
                return _lastInventoryId;
            }
        }
    }

    public InventoryPage(InventoryService inventoryService, AppointmentTypeService appointmentTypeService)
    {
        InitializeComponent();
        _inventoryService = inventoryService;
        _appointmentTypeService = appointmentTypeService;

        PossibleAppointmentTypes = _appointmentTypeService.GetItemsAsync().Result;
        Items = new ObservableCollection<Inventory>(_inventoryService.GetItemsAsync().Result);
        InventoryList.ItemsSource = Items;

        _lastInventoryId = Items.Count > 0 ? Items.Max(i => i.Id) : 0;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new InventoryEditPage(null, SaveItem));
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Inventory selected)
            await Navigation.PushAsync(new InventoryEditPage(selected, SaveItem));
    }

    private void SaveItem(Inventory item, bool isNew, bool isDeleted)
    {
        if (isDeleted)
        {
            var existing = Items.First(x => x.Id == item.Id);
            Items.Remove(existing);
            _inventoryService.DeleteItemAsync(item.Id);
            return;
        }

        if (isNew)
        {
            Items.Add(item);
            _inventoryService.AddItemAsync(item);
        }
        else
        {
            var existing = Items.First(x => x.Id == item.Id);
            var index = Items.IndexOf(existing);
            Items[index] = item;
            _inventoryService.UpdateItemAsync(item);
        }

    }
}
