using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Views;

public partial class CustomerPage : ContentPage
{
    public ObservableCollection<Customer> Items { get; set; }

    private readonly CustomerService _customerService;

    public CustomerPage(CustomerService customerService)
    {
        InitializeComponent();

        _customerService = customerService;

        Items = new ObservableCollection<Customer>(
            _customerService.GetItemsAsync().Result
        );

        CustomerList.ItemsSource = Items;
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
            Customer existing = Items.First(x => x.Id == item.Id);
            _ = Items.Remove(existing);
            _ = _customerService.DeleteItemAsync(item.Id);
            return;
        }

        if (isNew)
        {
            Items.Add(item);
            _ = _customerService.AddItemAsync(item);
        }
        else
        {
            Customer existing = Items.First(x => x.Id == item.Id);
            int index = Items.IndexOf(existing);
            Items[index] = item;
            _ = _customerService.UpdateItemAsync(item);
        }
    }
}
