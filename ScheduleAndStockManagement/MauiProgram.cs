using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using ScheduleAndStockManagement.Views;
using Syncfusion.Maui.Core.Hosting;


namespace ScheduleAndStockManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            MauiAppBuilder builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.ConfigureSyncfusionCore();
            builder.UseMicrocharts();

            builder.Services.AddDbContext<AppDbContext>();

            builder.Services.AddSingleton<IGeneralRepository<InventoryItemType>, InventoryItemTypeRepository>();
            builder.Services.AddSingleton<IGeneralRepository<InventoryItemTransaction>, InventoryItemTransactionRepository>();
            builder.Services.AddSingleton<IGeneralRepository<AppointmentType>, AppointmentTypeRepository>();
            builder.Services.AddSingleton<IGeneralRepository<Customer>, CustomerRepository>();
            builder.Services.AddSingleton<IGeneralRepository<Meeting>, MeetingRepository>();

            builder.Services.AddSingleton<InventoryItemTypeService>();
            builder.Services.AddSingleton<InventoryItemTransactionService>();
            builder.Services.AddSingleton<AppointmentTypeService>();
            builder.Services.AddSingleton<CustomerService>();
            builder.Services.AddSingleton<MeetingService>();

            builder.Services.AddTransient<InventoryItemTypesPage>();
            builder.Services.AddTransient<InventoryItemTransactionsPage>();
            builder.Services.AddTransient<AppointmentTypePage>();
            builder.Services.AddTransient<CustomerPage>();
            builder.Services.AddTransient<EditMeetingPage>();
            builder.Services.AddTransient<SchedulingAssistantPage>();
            builder.Services.AddTransient<ChartSelectionPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
