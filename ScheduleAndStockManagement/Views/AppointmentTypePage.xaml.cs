using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ScheduleAndStockManagement.Views;

public partial class AppointmentTypePage : ContentPage
{
    private readonly AppointmentTypeService _service;
    internal ObservableCollection<AppointmentType> Items { get; set; } = [];
    private static readonly object appointmentLock = new();

    public static int NextAppointmentId
    {
        get
        {
            lock (appointmentLock)
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

    public static IList PossibleBackgroundColors { get; } = GetAllSolidColorBrushes();

    public static List<string> GetAllSolidColorBrushes()
    {
        return typeof(Colors)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(Color))
            .Select(f => f.Name)
            .ToList();
    }

    public AppointmentTypePage(AppointmentTypeService appointmentTypeService)
    {
        InitializeComponent();
        _service = appointmentTypeService;

        Items = new ObservableCollection<AppointmentType>(_service.GetItemsAsync().Result);
        AppointmentTypeList.ItemsSource = Items;
        NextAppointmentId = Items.Count > 0 ? Items.Max(i => i.Id) : 0;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AppointmentTypeEditPage(null, SaveItem));
    }

    private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is AppointmentType selected)
        {
            await Navigation.PushAsync(new AppointmentTypeEditPage(selected, SaveItem));
        }
    }

    private void SaveItem(AppointmentType item, bool isNew, bool isDeleted)
    {
        if (isDeleted)
        {
            AppointmentType existing = Items.First(x => x.Id == item.Id);
            _ = Items.Remove(existing);
            _ = _service.DeleteItemAsync(item.Id);
            return;
        }

        if (isNew)
        {
            Items.Add(item);
            _ = _service.AddItemAsync(item);
        }
        else
        {
            AppointmentType existing = Items.First(x => x.Id == item.Id);
            int index = Items.IndexOf(existing);
            Items[index] = item;
            _ = _service.UpdateItemAsync(item);
        }

    }
}
