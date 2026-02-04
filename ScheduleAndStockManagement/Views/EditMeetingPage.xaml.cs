using Microsoft.Maui.Controls;
using ScheduleAndStockManagement.Models;
using System.ComponentModel;

namespace ScheduleAndStockManagement.Views;

public partial class EditMeetingPage : ContentPage, INotifyPropertyChanged
{
    private readonly Action<Meeting, bool, bool> saveCallback;
    private readonly bool isNew;
    public  Meeting MeetingItem { get; set; } = new Meeting();
    public List<Customer> Customers { get; set; }
    public List<AppointmentType> AppointmentTypes { get; set; }
    public Customer SelectedCustomer { get; set; }
    public AppointmentType SelectedAppointmentType { get; set; }

    public EditMeetingPage(DateTime startTime, Action<Meeting, bool, bool> saveCallback)
    {
        InitializeComponent();
        this.isNew = true;
        this.saveCallback = saveCallback;
        this.LoadStartDateTime(startTime);
        this.LoadEndDateTime(startTime.AddHours(1));
        this.LoadDropdownItems();
        this.LoadEntryValues();
        this.BindingContext = this;
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
        this.BindingContext = this;
    }

    public void LoadDropdownItems()
    {
        Customers = SchedulingAssistantPage.Customers;
        AppointmentTypes = SchedulingAssistantPage.AppointmentTypes;
        SelectedCustomer = MeetingItem.Customer;
        SelectedAppointmentType = MeetingItem.AppointmentType;
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
        if (string.IsNullOrEmpty(this.Price.Text))
        {
            this.Price.Text = "0"; 
        }

        if (string.IsNullOrEmpty(this.TipsValue.Text))
        {
            this.TipsValue.Text = "0";
        }

        if (!this.AppointmentTypes.Contains(SelectedAppointmentType))
        {
            await DisplayAlertAsync("Invalid Appointment Type", "Please select a valid appointment type!", "OK");
            return;
        }

        if (!this.Customers.Contains(SelectedCustomer))
        {
            await DisplayAlertAsync("Invalid Customer", "Please select a valid Customer!", "OK");
            return;
        }

        MeetingItem.From = this.fromDate.Date.Value.Add(this.fromTime.Time.Value);
        MeetingItem.To = this.toDate.Date.Value.Add(this.toTime.Time.Value);
        MeetingItem.AppointmentType = SelectedAppointmentType;
        MeetingItem.Customer = SelectedCustomer;
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

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if(SelectedCustomer is null)
        {
            await DisplayAlertAsync("No Customer Selected", "Please select a valid Customer to view details!", "OK");
            return;
        }

        var detailsPage = new CustomerDetailsPage(SelectedCustomer);
        await Navigation.PushAsync(detailsPage);
    }
}