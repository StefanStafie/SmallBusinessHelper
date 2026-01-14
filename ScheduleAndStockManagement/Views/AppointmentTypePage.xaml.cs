using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections;
using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Views;

public partial class AppointmentTypePage : ContentPage
{
    private AppointmentTypeService _service;
    internal ObservableCollection<AppointmentType> Items { get; set; } = new ObservableCollection<AppointmentType>();
    private static object appointmentLock = new object();
    private static int _lastAppointmentId = 0;
    public static int NextAppointmentId
    {
        get
        {
            lock (appointmentLock)
            {
                _lastAppointmentId++;
                return _lastAppointmentId;
            }
        }
    }

    public static IList PossibleBackgroundColors { get; } = new List<string> { "Red", "Green", "Blue", "Black", "Yellow", "Orange", "Violet" };

    public AppointmentTypePage(AppointmentTypeService appointmentTypeService)
    {
        InitializeComponent();
        _service = appointmentTypeService;

        Items = new ObservableCollection<AppointmentType>(_service.GetItemsAsync().Result);
        AppointmentTypeList.ItemsSource = Items;
        _lastAppointmentId = Items.Count > 0 ? Items.Max(i => i.Id) : 0;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppointmentTypeEditPage(null, SaveItem));
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is AppointmentType selected)
            await Navigation.PushAsync(new AppointmentTypeEditPage(selected, SaveItem));
    }

    private void SaveItem(AppointmentType item, bool isNew, bool isDeleted)
    {
        if(isDeleted)
        {
            var existing = Items.First(x => x.Id == item.Id);
            Items.Remove(existing);
            _service.DeleteItemAsync(item.Id);
            return;
        }

        if (isNew)
        {
            Items.Add(item);
            _service.AddItemAsync(item);        
        }
        else
        {
            var existing = Items.First(x => x.Id == item.Id);
            var index = Items.IndexOf(existing);
            Items[index] = item;
            _service.UpdateItemAsync(item);
        }

    }
}
