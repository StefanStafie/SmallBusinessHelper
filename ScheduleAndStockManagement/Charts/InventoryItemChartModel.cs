using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleAndStockManagement.Charts
{
    internal class InventoryItemChartModel
    {
        public string ItemName { get; set; } = string.Empty;
        public int RemainingStock { get; set; }
        public double AverageProfit { get; set; }
        public int TotalProfit { get; set; }
    }
}
