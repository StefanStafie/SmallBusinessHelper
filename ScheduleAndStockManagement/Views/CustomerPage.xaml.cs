using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Views;

public partial class CustomerPage : ContentPage
{
    internal ObservableCollection<Customer> Items { get; set; }
        = new ObservableCollection<Customer>();

    private readonly CustomerService _customerService;

    private static object customerLock = new object();
    private static int _lastCustomerId = 0;

    public static int NextCustomerId
    {
        get
        {
            lock (customerLock)
            {
                _lastCustomerId++;
                return _lastCustomerId;
            }
        }
    }

    public CustomerPage(CustomerService customerService)
    {
        InitializeComponent();

        _customerService = customerService;

        Items = new ObservableCollection<Customer>(
            _customerService.GetItemsAsync().Result
        );

        CustomerList.ItemsSource = Items;

        _lastCustomerId = Items.Count > 0
            ? Items.Max(c => c.Id)
            : 0;
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(
            new CustomerEditPage(null, SaveItem)
        );
    }

    private async void OnCustomerSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Customer selected)
        {
            await Navigation.PushAsync(
                new CustomerEditPage(selected, SaveItem)
            );
        }
    }

    private void SaveItem(Customer item, bool isNew, bool isDeleted)
    {
        if (isDeleted)
        {
            var existing = Items.First(x => x.Id == item.Id);
            Items.Remove(existing);
            _customerService.DeleteItemAsync(item.Id);
            return;
        }

        if (isNew)
        {
            Items.Add(item);
            _customerService.AddItemAsync(item);
        }
        else
        {
            var existing = Items.First(x => x.Id == item.Id);
            var index = Items.IndexOf(existing);
            Items[index] = item;
            _customerService.UpdateItemAsync(item);
        }
    }
}
