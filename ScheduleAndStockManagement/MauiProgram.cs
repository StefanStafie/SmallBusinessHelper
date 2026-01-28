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

            builder.Services.AddDbContext<AppDbContext>();

            builder.Services.AddSingleton<IGeneralRepository<Inventory>, InventoryRepository>();
            builder.Services.AddSingleton<IGeneralRepository<AppointmentType>, AppointmentTypeRepository>();
            builder.Services.AddSingleton<IGeneralRepository<Customer>, CustomerRepository>();
            builder.Services.AddSingleton<IGeneralRepository<Meeting>, MeetingRepository>();

            builder.Services.AddSingleton<InventoryService>();
            builder.Services.AddSingleton<AppointmentTypeService>();
            builder.Services.AddSingleton<CustomerService>();
            builder.Services.AddSingleton<MeetingService>();

            builder.Services.AddTransient<InventoryPage>();
            builder.Services.AddTransient<AppointmentTypePage>();
            builder.Services.AddTransient<CustomerPage>();
            builder.Services.AddTransient<EditMeetingPage>();
            builder.Services.AddTransient<SchedulingAssistant>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
