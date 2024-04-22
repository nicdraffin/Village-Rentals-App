using Microsoft.Extensions.Logging;
using Utility;
using Village_Rentals_App.Data;
using Village_Rentals_App.Model;

namespace Village_Rentals_App
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseContext>();
            builder.Services.AddSingleton<CustomersViewModel>();
            builder.Services.AddSingleton<Customer>();
            builder.Services.AddSingleton<Rental>();
            builder.Services.AddSingleton<RentalsViewModel>();
            builder.Services.AddSingleton<Inventory>();
            builder.Services.AddSingleton<InventoryViewModel>();

            return builder.Build();
        }
    }
}
