using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ScheduleAndStockManagement.Views;

public partial class SchedulingAssistant : ContentPage, INotifyPropertyChanged
{
    private DateTime _selectedDate = DateTime.Today;
  
    public DateTime SelectedDate
    {
        get => _selectedDate;
        set
        {
            if (_selectedDate == value) return;
            _selectedDate = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

