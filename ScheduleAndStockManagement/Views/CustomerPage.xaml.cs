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

    private async void OnCustomerTapped(object sender, TappedEventArgs e)
    {
        int selectedId = (CustomerList.SelectedItem as Customer).Id;
        var selected = Items.First(x => x.Id == selectedId);
        await Navigation.PushAsync(
                new CustomerEditPage(selected, SaveItem)
            );
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

    private void OnFilterChanged(object sender, TextChangedEventArgs e)
    {
        string nameFilter = FilterName.Text?.ToLower() ?? "";
        string phoneFilter = FilterPhone.Text?.ToLower() ?? "";
        string dateFilter = FilterDate.Text?.ToLower() ?? "";

        var filtered = Items.Where(item =>
            (string.IsNullOrWhiteSpace(nameFilter) || item.Name.ToLower().Contains(nameFilter)) &&
            (string.IsNullOrWhiteSpace(phoneFilter) || (item.Phone?.ToLower().Contains(phoneFilter) ?? false)) &&
            (string.IsNullOrWhiteSpace(dateFilter) || item.AddedAt.ToString("dd.MM.yyyy").Contains(dateFilter))
        ).ToList();

        CustomerList.ItemsSource = filtered;
    }

}
