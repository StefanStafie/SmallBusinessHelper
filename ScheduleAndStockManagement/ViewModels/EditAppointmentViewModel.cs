using ScheduleAndStockManagement.Enums;
using ScheduleAndStockManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace ScheduleAndStockManagement.ViewModels
{
    public class EditAppointmentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public List<AppointmentType> AppointmentTypes =>
            Enum.GetValues(typeof(AppointmentType)).Cast<AppointmentType>().ToList();

        private Appointment _appointment;

        public bool IsEditing => _appointment.Id != 0;

        public string Customer
        {
            get => _appointment.Customer;
            set { _appointment.Customer = value; OnPropertyChanged(nameof(Customer)); }
        }

        public AppointmentType Type
        {
            get => _appointment.Type;
            set { _appointment.Type = value; OnPropertyChanged(nameof(Type)); }
        }

        public DateTime Start
        {
            get => _appointment.Start;
            set
            {
                _appointment.Start = new DateTime(
                    value.Year, value.Month, value.Day,
                    Start.Hour, Start.Minute, 0);
                OnPropertyChanged(nameof(Start));
            }
        }

        public TimeSpan StartTimeOnly
        {
            get => _appointment.Start.TimeOfDay;
            set
            {
                _appointment.Start = Start.Date.Add(value);
                OnPropertyChanged(nameof(StartTimeOnly));
            }
        }

        public int Price
        {
            get => _appointment.Price;
            set { _appointment.Price = value; OnPropertyChanged(nameof(Price)); }
        }

        public int Tip
        {
            get => _appointment.Tip;
            set { _appointment.Tip = value; OnPropertyChanged(nameof(Tip)); }
        }

        public string Notes
        {
            get => _appointment.Notes;
            set { _appointment.Notes = value; OnPropertyChanged(nameof(Notes)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

        public EditAppointmentViewModel()
        {
            SaveCommand = new Command(async () => await Save());
            DeleteCommand = new Command(async () => await Delete());
        }

        public void CreateNew(DateTime date)
        {
            _appointment = new Appointment
            {
                Start = date,
                End = date.AddHours(1),
                Customer = string.Empty
            };
            OnPropertyChanged(null);
        }

        public void LoadExisting(Appointment appt)
        {
            _appointment = appt;
            OnPropertyChanged(null);
        }

        private async Task Save()
        {
            if (string.IsNullOrWhiteSpace(Customer))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Customer name is required", "OK");
                return;
            }

            await App.Database.SaveAppointmentAsync(_appointment);
            await Shell.Current.GoToAsync(".."); // close page
        }

        private async Task Delete()
        {
            bool confirm = await App.Current.MainPage.DisplayAlert(
                "Confirm",
                "Delete this appointment?",
                "Yes", "No");

            if (!confirm)
                return;

            await App.Database.DeleteAppointmentAsync(_appointment);
            await Shell.Current.GoToAsync("..");
        }
    }

}
