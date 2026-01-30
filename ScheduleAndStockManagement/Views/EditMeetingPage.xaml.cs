using Microsoft.Maui.Controls;
using ScheduleAndStockManagement.Models;
using System.ComponentModel;

namespace ScheduleAndStockManagement.Views;

public partial class EditMeetingPage : ContentPage, INotifyPropertyChanged
{
    private readonly Action<Meeting, bool, bool> saveCallback;
    private readonly bool isNew;
    public  Meeting MeetingItem { get; set; } = new Meeting();
    public List<Customer> Customers { get; set; } = SchedulingAssistantPage.Customers;
    public List<AppointmentType> AppointmentTypes { get; set; } = SchedulingAssistantPage.AppointmentTypes;

    public EditMeetingPage(DateTime startTime, Action<Meeting, bool, bool> saveCallback)
    {
        InitializeComponent();
        this.isNew = true;
        this.saveCallback = saveCallback;
        this.LoadStartDateTime(startTime);
        this.LoadEndDateTime(startTime.AddHours(1));
        this.LoadDropdownItems();
        this.LoadEntryValues();
    }

    private void LoadEntryValues()
    {
        this.Price.Text = MeetingItem.PriceLei.ToString();
        this.TipsValue.Text = MeetingItem.TipAmount.ToString();
        this.Notes.Text = MeetingItem.Notes;
    }

    public EditMeetingPage(Meeting meeting, Action<Meeting, bool, bool> saveCallback)
    {
        InitializeComponent();
        MeetingItem = meeting;
        this.isNew = false;
        this.saveCallback = saveCallback;
        this.LoadStartDateTime(meeting.From);
        this.LoadEndDateTime(meeting.To);
        this.LoadDropdownItems();
        this.LoadEntryValues();
    }

    public void LoadDropdownItems()
    {
        Customers = SchedulingAssistantPage.Customers;
        this.CustomerPicker.ItemsSource = Customers;
        if(MeetingItem.Customer is not null)
        {
            this.CustomerPicker.SelectedItem = MeetingItem.Customer;
        }
        else
        {
            this.CustomerPicker.SelectedIndex = 0;
        }

        AppointmentTypes = SchedulingAssistantPage.AppointmentTypes;
        this.AppointmentTypePicker.ItemsSource = AppointmentTypes;
        if(MeetingItem.AppointmentType is not null)
        {
            this.AppointmentTypePicker.SelectedItem = MeetingItem.AppointmentType;
        }
        else
        {
            this.AppointmentTypePicker.SelectedIndex = 0;
        }
    }

    public void LoadStartDateTime(DateTime start)
    {
        this.fromDate.Date = start.Date;
        this.fromTime.Time = start.TimeOfDay;
    }

    public void LoadEndDateTime(DateTime end)
    {
        this.toDate.Date = end.Date;
        this.toTime.Time = end.TimeOfDay;
    }

    void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        AppointmentType selectedItem = (AppointmentType)picker.SelectedItem;

        if (selectedItem is not null)
        {
            this.Price.Text = selectedItem.PriceLei.ToString();
            this.toDate.Date = this.fromDate.Date;
            this.toTime.Time = this.fromTime.Time.Value.Add(TimeSpan.FromMinutes(selectedItem.DurationMinutes) );
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if(this.CustomerPicker.SelectedItem == null || this.AppointmentTypePicker.SelectedItem == null)
        {
            await DisplayAlertAsync("Error", "Please select both a customer and an appointment type.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(this.Price.Text))
        {
            this.Price.Text = "0"; 
        }

        if (string.IsNullOrEmpty(this.TipsValue.Text))
        {
            this.TipsValue.Text = "0";
        }

        MeetingItem.From = this.fromDate.Date.Value.Add(this.fromTime.Time.Value);
        MeetingItem.To = this.toDate.Date.Value.Add(this.toTime.Time.Value);
        MeetingItem.AppointmentType = (AppointmentType)this.AppointmentTypePicker.SelectedItem;
        MeetingItem.Customer = (Customer)this.CustomerPicker.SelectedItem;
        MeetingItem.PriceLei = int.Parse(this.Price.Text);
        MeetingItem.TipAmount = int.Parse(this.TipsValue.Text);
        MeetingItem.Notes = this.Notes.Text;


        saveCallback(MeetingItem, isNew, false);
        _ = await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        saveCallback(MeetingItem, true, true);
        _ = await Navigation.PopAsync();
    }
}