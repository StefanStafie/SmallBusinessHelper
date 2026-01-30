using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace ScheduleAndStockManagement.Views;

public partial class SchedulingAssistantPage : ContentPage
{
    private readonly CustomerService _customerService;
    private readonly AppointmentTypeService _appointmentTypeService;
    private readonly MeetingService _meetingService;

    public ObservableCollection<Meeting> Meetings { get; set; }
    public static List<Customer> Customers { get; set; }
    public static List<AppointmentType> AppointmentTypes { get; set; }
    private static readonly object meetingLock = new();

    public static int NextMeetingId
    {
        get
        {
            lock (meetingLock)
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
    
    public SchedulingAssistantPage(CustomerService customerService, AppointmentTypeService appointmentTypeService, MeetingService meetingService)
    {
        InitializeComponent();
        scheduler.AppointmentDrop += OnSchedulerAppointmentDrop;

        _meetingService = meetingService;
        _appointmentTypeService = appointmentTypeService;
        _customerService = customerService;

        Customers = _customerService.GetItemsAsync().Result;
        AppointmentTypes = _appointmentTypeService.GetItemsAsync().Result;
        Meetings = new ObservableCollection<Meeting>(_meetingService.GetItemsAsync().Result);

        BindingContext = this;
        scheduler.AllowAppointmentDrag = true;
    }
    private void OnSchedulerAppointmentDrop(object? sender, AppointmentDropEventArgs e)
    {
        var appointment = e.Appointment;
        e.Cancel = false;

        var meeting = Meetings.First(x=>x.Id == (int)appointment.Id);
        var difference = e.DropTime - meeting.From;

        meeting.From = e.DropTime;
        meeting.To = meeting.To.Add(difference);
        _meetingService.UpdateItemAsync(meeting);
    }

    private async void OnSchedulerDoubleTapped(object sender, SchedulerDoubleTappedEventArgs e)
    {
        // empty cells
        if (e.Element == SchedulerElement.SchedulerCell)
        {
            if (e.Date is null)
            {
                return;
            }

            DateTime startTime = e.Date.Value;
            await Navigation.PushAsync(new EditMeetingPage(startTime, Saveitem));
        }

        // appointment cells
        if (e.Element == SchedulerElement.Appointment)
        {
            Meeting selectedMeeting = (Meeting)e.Appointments[0];
            await Navigation.PushAsync(new EditMeetingPage(selectedMeeting, Saveitem));
        }
    }

    private void Saveitem(Meeting item, bool isNew, bool isDeleted)
    {
        if (isDeleted)
        {
            Meeting existing = Meetings.First(x => x.Id == item.Id);
            _ = Meetings.Remove(existing);
            _ = _meetingService.DeleteItemAsync(item.Id);
            return;
        }
        else
        {
            if (isNew)
            {
                Meetings.Add(item);
                _meetingService.AddItemAsync(item);
            }
            else
            {
                Meeting existing = Meetings.First(x => x.Id == item.Id);
                var index = Meetings.IndexOf(existing);
                Meetings[index] = item;
                _ = _meetingService.UpdateItemAsync(item);
            }
        }
    }
}

