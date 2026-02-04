using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleAndStockManagement.Models
{
    public partial class CustomerFile : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        public string fileName;

        [ObservableProperty]
        public string filePath;

        public int CustomerId { get; set; } 
    }
}
