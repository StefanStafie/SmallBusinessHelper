using Microcharts;
using ScheduleAndStockManagement.Enums;
using ScheduleAndStockManagement.Intefaces;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using SkiaSharp;
using Syncfusion.Maui.Charts;
using System.Net.ServerSentEvents;

namespace ScheduleAndStockManagement.Views;

public partial class ChartSelectionPage : ContentPage
{
    private readonly CustomerService customerService;
    private readonly AppointmentTypeService appointmentTypeService;
    private readonly InventoryItemTransactionService inventoryItemTransactionService;
    private readonly MeetingService meetingService;
    private readonly InventoryItemTypeService inventoryService;

    private readonly List<Meeting> meetings;
    private readonly List<Customer> customers;
    private readonly List<InventoryItemType> inventoryItemTypes;
    private readonly List<InventoryItemTransaction> inventoryItemTransactions;
    private readonly List<AppointmentType> appointmentTypes;

    private readonly Dictionary<ChartType, Action> chartActions;

    public ChartSelectionPage(
        CustomerService customerService, 
        AppointmentTypeService appointmentTypeService, 
        MeetingService meetingService, 
        InventoryItemTypeService inventoryService,
        InventoryItemTransactionService inventoryItemTransactionService)
    {
        InitializeComponent();
        this.meetingService = meetingService;
        this.customerService = customerService;
        this.inventoryService = inventoryService;
        this.appointmentTypeService = appointmentTypeService;
        this.inventoryItemTransactionService = inventoryItemTransactionService;


        meetings = this.meetingService.GetItemsAsync().Result;
        customers = this.customerService.GetItemsAsync().Result;
        inventoryItemTypes = this.inventoryService.GetItemsAsync().Result;
        inventoryItemTransactions = this.inventoryItemTransactionService.GetItemsAsync().Result;
        appointmentTypes = this.appointmentTypeService.GetItemsAsync().Result;

        var appointmentsSource = appointmentTypes;
        appointmentsSource.Insert(0, new AppointmentType() { Id = -1, EventName = "None" });
        this.AppointmentTypePicker.ItemsSource = appointmentsSource;
        this.AppointmentTypePicker.SelectedIndex = 0;

        chartActions = new Dictionary<ChartType, Action>()
            {
                { ChartType.MeetingsPerAppointmentType, DisplayMeetingsPerAppointmentType },
                { ChartType.EarningsPerAppointmentType, DisplayEarningsPerAppointmentType },
                { ChartType.MeetingsPerMonth, DisplayMeetingsPerMonth },
                { ChartType.TipsVsOtherSales, DisplayTipsVsSales },
                { ChartType.InventoryPerItem, DisplayInventoryPerItem },
                { ChartType.EarningsVsSpendingDaily, DisplayEarningVsSpendingDaily },
                { ChartType.EarningsVsSpendingEvolution, DisplayEarningVsSpendingEvolution },

            };

        ChartPicker.ItemsSource = chartActions.Keys.ToList();

        StartDatePicker.Date = DateTime.Today.AddMonths(-1);
        EndDatePicker.Date = DateTime.Today;
    }

    private void DisplayMeetingsPerAppointmentType()
    {
    }
    private void DisplayEarningsPerAppointmentType()
    {
    }
    private void DisplayMeetingsPerMonth()
    {
    }
    private void DisplayTipsVsSales()
    {
    }
    private void DisplayInventoryPerItem()
    {
    }



    public void DisplayEarningVsSpendingDaily()
    {
        SfCartesianChart chart = new SfCartesianChart();

        DateTimeAxis primaryAxis = new DateTimeAxis();
        chart.XAxes.Add(primaryAxis);

        NumericalAxis secondaryAxis = new NumericalAxis();
        chart.YAxes.Add(secondaryAxis);

        var selectedAppointmentType = (AppointmentType)this.AppointmentTypePicker.SelectedItem;
        Dictionary<DateTime, int> meetingsEarnings = meetings
        .Where(m=> this.AppointmentTypePicker.SelectedIndex == 0 || m.AppointmentType == selectedAppointmentType)
        .GroupBy(m => m.EarningDateTime())
        .ToDictionary(
            g => g.Key,
            g => g.Sum(m => m.CalculateEarnings())
        );

        Dictionary<DateTime, int> inventoryEarnings = inventoryItemTransactions
            .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.InventoryItemType.AppointmentType == selectedAppointmentType)
            .GroupBy(m => m.EarningDateTime())
            .ToDictionary(
                g => g.Key,
                g => g.Sum(m => m.CalculateEarnings())
            );

        var totalEarnings = meetingsEarnings.Union(inventoryEarnings)
            .GroupBy(kvp => kvp.Key)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(kvp => kvp.Value)
            );

        Dictionary<DateTime, int> inventorySpendings = inventoryItemTransactions
            .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.InventoryItemType.AppointmentType == selectedAppointmentType)
            .GroupBy(m => m.SpendingDateTime())
            .ToDictionary(
                g => g.Key,
                g => g.Sum(m => m.CalculateSpendings())
            );
        

        if(this.StartDatePicker.Date.HasValue && this.EndDatePicker.Date.HasValue)
        {
            inventorySpendings = this.FilterByDate(this.StartDatePicker.Date.Value, this.EndDatePicker.Date.Value, inventorySpendings);
            totalEarnings = this.FilterByDate(this.StartDatePicker.Date.Value, this.EndDatePicker.Date.Value, totalEarnings);
        }

       

        LineSeries series1 = new LineSeries()
        {
            ItemsSource = totalEarnings.OrderBy(x=>x.Key),
            XBindingPath = "Key",
            YBindingPath = "Value",
        };

        LineSeries series2 = new LineSeries()
        {
            ItemsSource = inventorySpendings.OrderBy(x=>x.Key),
            XBindingPath = "Key",
            YBindingPath = "Value",
            Fill = new SolidColorBrush(Colors.Red)
        };

        chart.Series.Add(series1);
        chart.Series.Add(series2);

        Navigation.PushAsync(new ChartDisplayPage(chart));
    }

    public void DisplayEarningVsSpendingEvolution()
    {
        SfCartesianChart chart = new SfCartesianChart();
        
        DateTimeAxis primaryAxis = new DateTimeAxis();
        chart.XAxes.Add(primaryAxis);

        NumericalAxis secondaryAxis = new NumericalAxis();
        chart.YAxes.Add(secondaryAxis);

        var selectedAppointmentType = (AppointmentType)this.AppointmentTypePicker.SelectedItem;
        Dictionary<DateTime, int> meetingsEarnings = meetings
        .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.AppointmentType == selectedAppointmentType)
        .GroupBy(m => m.EarningDateTime())
        .ToDictionary(
            g => g.Key,
            g => g.Sum(m => m.CalculateEarnings())
        );

        Dictionary<DateTime, int> inventoryEarnings = inventoryItemTransactions
            .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.InventoryItemType.AppointmentType == selectedAppointmentType)
            .GroupBy(m => m.EarningDateTime())
            .ToDictionary(
                g => g.Key,
                g => g.Sum(m => m.CalculateEarnings())
            );

        var totalEarnings = meetingsEarnings.Union(inventoryEarnings)
            .GroupBy(kvp => kvp.Key)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(kvp => kvp.Value)
            );

        Dictionary<DateTime, int> inventorySpendings = inventoryItemTransactions
            .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.InventoryItemType.AppointmentType == selectedAppointmentType)
            .GroupBy(m => m.SpendingDateTime())
            .ToDictionary(
                g => g.Key,
                g => g.Sum(m => m.CalculateSpendings())
            );


        if (this.StartDatePicker.Date.HasValue && this.EndDatePicker.Date.HasValue)
        {
            inventorySpendings = this.FilterByDate(this.StartDatePicker.Date.Value, this.EndDatePicker.Date.Value, inventorySpendings);
            totalEarnings = this.FilterByDate(this.StartDatePicker.Date.Value, this.EndDatePicker.Date.Value, totalEarnings);
        }

        var sum = 0;
        //totalEarnings = totalEarnings.OrderBy(x => x.Key).ToList().ForEach(kvp => { kvp.Value = kvp.Value + sum; sum = kvp.Value; });

        LineSeries series1 = new LineSeries()
        {
            ItemsSource = totalEarnings.OrderBy(x => x.Key),
            XBindingPath = "Key",
            YBindingPath = "Value",
        };

        LineSeries series2 = new LineSeries()
        {
            ItemsSource = inventorySpendings.OrderBy(x => x.Key),
            XBindingPath = "Key",
            YBindingPath = "Value",
            Fill = new SolidColorBrush(Colors.Red)
        };

        chart.Series.Add(series1);
        chart.Series.Add(series2);

        Navigation.PushAsync(new ChartDisplayPage(chart));
    }

    private Dictionary<DateTime, int> FilterByDate(DateTime value1, DateTime value2, Dictionary<DateTime, int> dict)
    {
        return dict
            .Where(kvp => kvp.Key.Date >= value1.Date && kvp.Key.Date <= value2.Date)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private void ChartPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch(ChartPicker.SelectedItem)
        {
            case ChartType.EarningsVsSpendingDaily:
                
                break;
        }


    }

    private void OnDisplayClicked(object sender, EventArgs e)
    {
        chartActions[(ChartType)this.ChartPicker.SelectedItem].Invoke();
    }
}

