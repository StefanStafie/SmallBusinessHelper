using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;

namespace ScheduleAndStockManagement.Views;

public partial class GraphsAndReports: ContentPage
{
    private readonly CustomerService _customerService;
    private readonly AppointmentTypeService _appointmentTypeService;
    private readonly MeetingService _meetingService;
    private readonly InventoryItemTypeService _inventoryService;

    private readonly List<Meeting> _meetings;
    private readonly List<Customer> _customers;
    private readonly List<InventoryItemType> _inventory;
    private readonly List<AppointmentType> _appointmentType;

    public GraphsAndReports(CustomerService customerService, AppointmentTypeService appointmentTypeService, MeetingService meetingService, InventoryItemTypeService inventoryService)
    {
        InitializeComponent();
        _meetingService = meetingService;
        _customerService = customerService;
        _inventoryService = inventoryService;
        _appointmentTypeService = appointmentTypeService;
        
        _meetings = _meetingService.GetItemsAsync().Result;
        _customers = _customerService.GetItemsAsync().Result;
        _inventory = _inventoryService.GetItemsAsync().Result;
        _appointmentType = _appointmentTypeService.GetItemsAsync().Result;
    }
}

