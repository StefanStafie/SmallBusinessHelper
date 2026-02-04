using System.ComponentModel;
using System.Runtime.CompilerServices;
using ScheduleAndStockManagement.Enums;
using ScheduleAndStockManagement.Models;

namespace ScheduleAndStockManagement.Views;

public partial class InventoryItemTypeEditPage : ContentPage, INotifyPropertyChanged
{
    private readonly bool isNew;
    private readonly Action<InventoryItemType, bool, bool> saveCallback;

    private string name;
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(); }
    }

    private AppointmentType selectedAppointmentType;
    public AppointmentType SelectedAppointmentType
    {
        get => selectedAppointmentType;
        set { selectedAppointmentType = value; OnPropertyChanged(); }
    }

    private InventoryTypeDesignation selectedInventoryType;
    public InventoryTypeDesignation SelectedInventoryType
    {
        get => selectedInventoryType;
        set { selectedInventoryType = value; OnPropertyChanged(); }
    }

    public InventoryItemType InventoryItemType { get; private set; }

    public IList<AppointmentType> PossibleAppointmentTypes => InventoryItemTypesPage.PossibleAppointmentTypes;

    internal InventoryItemTypeEditPage(InventoryItemType? item, Action<InventoryItemType, bool, bool> saveCallback)
    {
        InitializeComponent();
        this.saveCallback = saveCallback;
        isNew = item == null;

        InventoryItemType = item ?? new InventoryItemType
        {
            Name = string.Empty,
            AppointmentType = InventoryItemTypesPage.PossibleAppointmentTypes.First(),
            Designation = InventoryTypeDesignation.Consumable,
        };

        // Initialize properties for binding
        Name = InventoryItemType.Name;
        SelectedAppointmentType = InventoryItemType.AppointmentType;
        SelectedInventoryType = InventoryItemType.Designation;

        BindingContext = this;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        // Update the model from bound properties
        InventoryItemType.Name = Name;
        InventoryItemType.AppointmentType = SelectedAppointmentType;
        InventoryItemType.Designation = SelectedInventoryType;

        saveCallback(InventoryItemType, isNew, false);
        _ = await Navigation.PopAsync();
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        saveCallback(InventoryItemType, true, true);
        _ = await Navigation.PopAsync();
    }
}
