using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Views;

public partial class CustomerDetailsPage : ContentPage
{
    private readonly Customer item;
    public ObservableCollection<CustomerFile> Items { get; set; }
    public ObservableCollection<Meeting> OtherMeetings { get; set; }

    public CustomerDetailsPage(Customer item)
    {
        InitializeComponent();
        this.item = item;
        Items = new ObservableCollection<CustomerFile>(item.customerFiles);
        OtherMeetings = new ObservableCollection<Meeting>(App.Services.GetService<MeetingService>().GetItemsForCustomer(item.id) ?? []);
        this.FilesList.ItemsSource = Items;
        LoadData();
        this.BindingContext = this;
    }

    private void LoadData()
    {
        NameEntry.Text = item.Name;
        PhoneEntry.Text = item.Phone;
        AddedAtDatePicker.Date = item.AddedAt.Date;
        AddedAtTimePicker.Time = item.AddedAt.TimeOfDay;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        _ = await Navigation.PopAsync();
    }

    private async void OnViewFileClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
        button.CommandParameter is CustomerFile file)
        {
            if (File.Exists(file.filePath))
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(file.filePath)
                });
            }
        }
    }
}
