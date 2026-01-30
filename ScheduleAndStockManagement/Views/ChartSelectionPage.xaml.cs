using Microcharts;
using ScheduleAndStockManagement.Charts;
using ScheduleAndStockManagement.Enums;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Core;
using System.Collections.ObjectModel;

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
                { ChartType.TipsVsOtherSales, DisplayTipsVsSales },
                { ChartType.InventoryTotalProfitPerItem, DisplayInventoryTotalProfitPerItem },
                { ChartType.InventoryAvgProfitPerItem, DisplayInventoryAvgProfitPerItem },
                { ChartType.InventoryCountPerItem, DisplayInventoryCountPerItem },
                { ChartType.EarningsVsSpendingDaily, DisplayEarningVsSpendingDaily },
                { ChartType.EarningsVsSpendingEvolution, DisplayEarningVsSpendingEvolution },

            };

        ChartPicker.ItemsSource = chartActions.Keys.ToList();
        ChartPicker.SelectedIndex = 0;

        StartDatePicker.Date = DateTime.Today.AddYears(-1);
        EndDatePicker.Date = DateTime.Today;
    }

    private void DisplayInventoryTotalProfitPerItem()
    {
        var inventoryChartData = new List<InventoryItemChartModel>();

        foreach (var itemType in inventoryItemTypes)
        {
            if (itemType.ForSale == false)
            {
                continue;
            }

            var transactions = inventoryItemTransactions
                .Where(t => t.InventoryItemType?.Id == itemType.Id)
                .ToList();

            int stock = transactions.Sum(t => t.WasSold ? -t.Quantity : t.Quantity);

            int totalSpent = transactions
                .Where(t => !t.WasSold)
                .Sum(t => t.CalculateSpendings());

            int totalEarned = transactions
                .Where(t => t.WasSold)
                .Sum(t => t.CalculateEarnings());

            int totalProfit = totalEarned - totalSpent;

            int soldCount = transactions.Count(t => t.WasSold);

            double avgProfit = soldCount > 0
                ? (double)totalProfit / soldCount
                : 0;

            inventoryChartData.Add(new InventoryItemChartModel
            {
                ItemName = itemType.Name,
                RemainingStock = stock,
                TotalProfit = totalProfit,
                AverageProfit = avgProfit
            });
        }


        var chart = this.CreateBaseBaseBarChart();

        chart.Series.Add(new ColumnSeries
        {
            Label = "Total Profit",
            ItemsSource = inventoryChartData,
            XBindingPath = "ItemName",
            YBindingPath = "TotalProfit",
        });

        Navigation.PushAsync(new ChartDisplayPage(chart));
    }

    private void DisplayInventoryAvgProfitPerItem()
    {
        var inventoryChartData = new List<InventoryItemChartModel>();

        foreach (var itemType in inventoryItemTypes)
        {
            if (itemType.ForSale == false)
            {
                continue;
            }

            var transactions = inventoryItemTransactions
                .Where(t => t.InventoryItemType?.Id == itemType.Id)
                .ToList();

            int stock = transactions.Sum(t => t.WasSold ? -t.Quantity : t.Quantity);

            int totalSpent = transactions
                .Where(t => !t.WasSold)
                .Sum(t => t.CalculateSpendings());

            int totalEarned = transactions
                .Where(t => t.WasSold)
                .Sum(t => t.CalculateEarnings());

            int totalProfit = totalEarned - totalSpent;

            int soldCount = transactions.Count(t => t.WasSold);

            double avgProfit = soldCount > 0
                ? (double)totalProfit / soldCount
                : 0;

            inventoryChartData.Add(new InventoryItemChartModel
            {
                ItemName = itemType.Name,
                RemainingStock = stock,
                TotalProfit = totalProfit,
                AverageProfit = avgProfit
            });
        }

        var chart = this.CreateBaseBaseBarChart();
        chart.Series.Add(new ColumnSeries
        {
            Label = "Average Profit",
            ItemsSource = inventoryChartData,
            XBindingPath = "ItemName",
            YBindingPath = "AverageProfit"
        });

        Navigation.PushAsync(new ChartDisplayPage(chart));
    }

    private void DisplayTipsVsSales()
    {
        var selectedAppointmentType = (AppointmentType)this.AppointmentTypePicker.SelectedItem;
        
        Dictionary<DateTime, int> meetingsPriceEarnings = meetings
        .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.AppointmentType == selectedAppointmentType)
        .GroupBy(m => m.EarningDateTime())
        .ToDictionary(
            g => g.Key,
            g => g.Sum(m => m.PriceLei)
        );

        Dictionary<DateTime, int> meetingsTipsEarnings = meetings
        .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.AppointmentType == selectedAppointmentType)
        .GroupBy(m => m.EarningDateTime())
        .ToDictionary(
            g => g.Key,
            g => g.Sum(m => m.TipAmount)
        );

        Dictionary<DateTime, int> inventoryEarnings = inventoryItemTransactions
            .Where(m => this.AppointmentTypePicker.SelectedIndex == 0 || m.InventoryItemType.AppointmentType == selectedAppointmentType)
            .GroupBy(m => m.EarningDateTime())
            .ToDictionary(
                g => g.Key,
                g => g.Sum(m => m.CalculateEarnings())
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
            inventoryEarnings = this.FilterByDate(this.StartDatePicker.Date.Value, this.EndDatePicker.Date.Value, inventoryEarnings);
            meetingsTipsEarnings = this.FilterByDate(this.StartDatePicker.Date.Value, this.EndDatePicker.Date.Value, meetingsTipsEarnings);
            meetingsPriceEarnings = this.FilterByDate(this.StartDatePicker.Date.Value, this.EndDatePicker.Date.Value, meetingsPriceEarnings);
        }

        int sum = 0;
        inventorySpendings = inventorySpendings
            .OrderBy(x => x.Key)
            .ToDictionary(
                x => x.Key,
                x => sum += x.Value
            );

        sum = 0;
        inventoryEarnings = inventoryEarnings
            .OrderBy(x => x.Key)
            .ToDictionary(
                x => x.Key,
                x => sum += x.Value
            );

        sum = 0;
        meetingsTipsEarnings = meetingsTipsEarnings
            .OrderBy(x => x.Key)
            .ToDictionary(
                x => x.Key,
                x => sum += x.Value
            );

        sum = 0;
        meetingsPriceEarnings = meetingsPriceEarnings
            .OrderBy(x => x.Key)
            .ToDictionary(
                x => x.Key,
                x => sum += x.Value
            );

        var chart = this.CreateBaseBaseBarChart();

        AddSeriesToChart(chart, inventorySpendings, Colors.Blue, "spent");
        AddSeriesToChart(chart, inventoryEarnings, Colors.Green, "sold");
        AddSeriesToChart(chart, meetingsTipsEarnings, Colors.Violet, "tips");
        AddSeriesToChart(chart, meetingsPriceEarnings, Colors.Red, "tariff");

        Navigation.PushAsync(new ChartDisplayPage(chart));
    }
    private void DisplayInventoryCountPerItem()
    {
        var inventoryChartData = new List<InventoryItemChartModel>();

        foreach (var itemType in inventoryItemTypes)
        {
            if(itemType.ForSale == false)
            {
                continue;
            }

            var transactions = inventoryItemTransactions
                .Where(t => t.InventoryItemType?.Id == itemType.Id)
                .ToList();

            int stock = transactions.Sum(t => t.WasSold ? -t.Quantity : t.Quantity);

            int totalSpent = transactions
                .Where(t => !t.WasSold)
                .Sum(t => t.CalculateSpendings());

            int totalEarned = transactions
                .Where(t => t.WasSold)
                .Sum(t => t.CalculateEarnings());

            int totalProfit = totalEarned - totalSpent;

            int soldCount = transactions.Count(t => t.WasSold);

            double avgProfit = soldCount > 0
                ? (double)totalProfit / soldCount
                : 0;

            inventoryChartData.Add(new InventoryItemChartModel
            {
                ItemName = itemType.Name,
                RemainingStock = stock,
                TotalProfit = totalProfit,
                AverageProfit = avgProfit
            });
        }

        var chart = this.CreateBaseBaseBarChart();
       
        chart.Series.Add(new ColumnSeries
        {
            Label = "Remaining Stock",
            ItemsSource = inventoryChartData,
            XBindingPath = "ItemName",
            YBindingPath = "RemainingStock"
        });

        Navigation.PushAsync(new ChartDisplayPage(chart));
    }

    public void DisplayEarningVsSpendingDaily()
    {

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

        totalEarnings = totalEarnings.OrderBy(x => x.Key).ToDictionary();
        inventorySpendings = inventorySpendings.OrderBy(x => x.Key).ToDictionary();

        SfCartesianChart chart = CreateBaseBaseLineChart();
        AddSeriesToChart(chart, totalEarnings, Colors.Green, "earned");
        AddSeriesToChart(chart, inventorySpendings, Colors.Red, "spent");
        Navigation.PushAsync(new ChartDisplayPage(chart));
    }

    private void AddSeriesToChart(SfCartesianChart chart, Dictionary<DateTime, int>  seriesKvp, Color color, string label)
    {

        LineSeries series = new LineSeries()
        {
            ItemsSource = seriesKvp,
            XBindingPath = "Key",
            YBindingPath = "Value",
            Fill = new SolidColorBrush(color),
            Label = label,
        };

        chart.Series.Add(series);
    }

    public void DisplayEarningVsSpendingEvolution()
    {

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

        SfCartesianChart chart = CreateBaseBaseLineChart();

        int sum = 0;
        totalEarnings = totalEarnings
            .OrderBy(x => x.Key)
            .ToDictionary(
                x => x.Key,
                x => sum += x.Value
            );

        sum = 0;
        inventorySpendings = inventorySpendings
            .OrderBy(x => x.Key)
            .ToDictionary(
                x => x.Key,
                x => sum += x.Value
            );

        AddSeriesToChart(chart, totalEarnings, Colors.Green, "earned");
        AddSeriesToChart(chart, inventorySpendings, Colors.Red, "spent");

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
        if(ChartPicker.SelectedItem == null)
        {
            DisplayAlertAsync("Error", "Please select a chart type.", "OK");
            return;
        }

        chartActions[(ChartType)this.ChartPicker.SelectedItem].Invoke();
    }

    private SfCartesianChart CreateBaseBaseLineChart()
    {
        var chart = new SfCartesianChart
        {
            Background = new SolidColorBrush(Colors.Black),
            Legend = new ChartLegend
            {
                IsVisible = true,
                Placement = LegendPlacement.Bottom
            }
        };

        DateTimeAxis primaryAxis = new DateTimeAxis
        {
            MajorGridLineStyle = new ChartLineStyle
            {
                StrokeWidth = 0.3,
                Stroke = new SolidColorBrush(Colors.DarkGray),
                StrokeDashArray = new DoubleCollection { 10, 10 }
            },
            LabelStyle = new ChartAxisLabelStyle
            {
                TextColor = Colors.WhiteSmoke,
                FontSize = 12
            }
        };

        NumericalAxis secondaryAxis = new NumericalAxis
        {
            MajorGridLineStyle = new ChartLineStyle
            {
                StrokeWidth = 0.3,
                Stroke = new SolidColorBrush(Colors.DarkGray),
                StrokeDashArray = new DoubleCollection { 10, 10 }
            },
            LabelStyle = new ChartAxisLabelStyle
            {
                TextColor = Colors.WhiteSmoke,
                FontSize = 12,
            }
        };

        chart.XAxes.Add(primaryAxis);
        chart.YAxes.Add(secondaryAxis);
        return chart;
    }

    private SfCartesianChart CreateBaseBaseBarChart()
    {
        var chart = new SfCartesianChart
        {
            Background = new SolidColorBrush(Colors.Black),
            Legend = new ChartLegend
            {
                IsVisible = true,
                Placement = LegendPlacement.Bottom
            }
        };

        chart.XAxes.Add(new CategoryAxis
        {
            Title = new ChartAxisTitle { Text = "Inventory Item" },
            LabelStyle = new ChartAxisLabelStyle
            {
                TextColor = Colors.WhiteSmoke,
                FontSize = 12,
            }
        });

        NumericalAxis secondaryAxis = new NumericalAxis
        {
            MajorGridLineStyle = new ChartLineStyle
            {
                StrokeWidth = 0.3,
                Stroke = new SolidColorBrush(Colors.DarkGray),
                StrokeDashArray = new DoubleCollection { 10, 10 }
            },
            LabelStyle = new ChartAxisLabelStyle
            {
                TextColor = Colors.WhiteSmoke,
                FontSize = 12,
            }
        };

        chart.YAxes.Add(secondaryAxis);
        return chart;
    }
}

