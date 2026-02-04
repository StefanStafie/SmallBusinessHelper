using CommunityToolkit.Mvvm.ComponentModel;
using ScheduleAndStockManagement.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;

namespace ScheduleAndStockManagement.Views;

public partial class CustomerEditPage : ContentPage
{
    private readonly Customer item;
    private readonly bool isNew;
    private readonly Action<Customer, bool, bool> saveCallback;
    public ObservableCollection<CustomerFile> Items { get; set; }

    internal CustomerEditPage(Customer? item, Action<Customer, bool, bool> saveCallback)
    {
        InitializeComponent();

        this.saveCallback = saveCallback;
        this.isNew = item == null;

        this.item = item ?? new Customer
        {
            Name = string.Empty,
            Phone = string.Empty,
            AddedAt = DateTime.Now,
            CustomerFiles = new List<CustomerFile>()
        };

        Items = new ObservableCollection<CustomerFile>(this.item.CustomerFiles);
        FilesList.ItemsSource = Items;
        LoadData();
    }

    private void LoadData()
    {
        NameEntry.Text = item.Name;
        PhoneEntry.Text = item.Phone;

        AddedAtDatePicker.Date = item.AddedAt.Date;
        AddedAtTimePicker.Time = item.AddedAt.TimeOfDay;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        item.Name = NameEntry.Text;
        item.Phone = PhoneEntry.Text;
        item.AddedAt = AddedAtDatePicker.Date.Value
                            .AddTicks(AddedAtTimePicker.Time.Value.Ticks);
        item.customerFiles = Items.ToList();
        saveCallback(item, isNew, false);
        _ = await Navigation.PopAsync();
    }

    private async void OnAddFileClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select a file"
            });

            if (result == null)
                return;

            var customerName = NameEntry.Text;
            var phoneNumber = PhoneEntry.Text;

            var safeCustomerName = SanitizeFolderName(customerName);
            var safePhone = SanitizeFolderName(phoneNumber);

            var customerFolderName = $"{safeCustomerName}_{safePhone}";

            var appFolder = FileSystem.AppDataDirectory;

            var customerFolderPath = Path.Combine(appFolder, "Customers", customerFolderName);

            Directory.CreateDirectory(customerFolderPath);

            var fileName = result.FileName;
            var destinationPath = Path.Combine(customerFolderPath, fileName);

            if (File.Exists(destinationPath))
            {
                var name = Path.GetFileNameWithoutExtension(fileName);
                var ext = Path.GetExtension(fileName);
                fileName = $"{name}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                destinationPath = Path.Combine(customerFolderPath, fileName);
            }

            using var sourceStream = await result.OpenReadAsync();
            using var destinationStream = File.Create(destinationPath);
            await sourceStream.CopyToAsync(destinationStream);

            Items.Add(new CustomerFile
            {
                FileName = fileName,
                FilePath = destinationPath
            });
            
            await DisplayAlertAsync("Success", "File saved successfully.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }


    private void OnRemoveFileClicked(object sender, EventArgs e)
    {
        if (sender is Button button &&
        button.CommandParameter is CustomerFile file)
        {
            Items.Remove(file);
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        saveCallback(this.item, true, true);
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

    private string SanitizeFolderName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');

        return name;
    }
}
