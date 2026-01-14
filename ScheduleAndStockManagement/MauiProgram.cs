using Microsoft.Extensions.Logging;
using ScheduleAndStockManagement.Data;
using ScheduleAndStockManagement.Models;
using ScheduleAndStockManagement.Services;
using ScheduleAndStockManagement.Views;


namespace ScheduleAndStockManagement
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddDbContext<AppDbContext>();

            builder.Services.AddSingleton<IGeneralRepository<Inventory>, InventoryRepository>();
            builder.Services.AddSingleton<IGeneralRepository<Appointment>, AppointmentRepository>();
            builder.Services.AddSingleton<IGeneralRepository<AppointmentType>, AppointmentTypeRepository>();
            builder.Services.AddSingleton<IGeneralRepository<Customer>, CustomerRepository>();

            builder.Services.AddSingleton<InventoryService>();
            builder.Services.AddSingleton<AppointmentService>();
            builder.Services.AddSingleton<AppointmentTypeService>();
            builder.Services.AddSingleton<CustomerService>();

            builder.Services.AddTransient<InventoryPage>();
            builder.Services.AddTransient<AppointmentTypePage>();
            builder.Services.AddTransient<CustomerPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
