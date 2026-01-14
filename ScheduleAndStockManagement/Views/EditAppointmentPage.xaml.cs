using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;


public partial class EditAppointmentPage : ContentPage, IQueryAttributable
{
    public EditAppointmentPage()
    {
        InitializeComponent();
        BindingContext = new EditAppointmentViewModel();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Appointment"))
        {
            ((EditAppointmentViewModel)BindingContext)
                .LoadExisting((Appointment)query["Appointment"]);
        }
        else if (query.ContainsKey("Date"))
        {
            ((EditAppointmentViewModel)BindingContext)
                .CreateNew((DateTime)query["Date"]);
        }
    }
}

