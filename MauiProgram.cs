using Microsoft.Extensions.Logging;
using Makara.Data;
using Makara.ViewModels;

namespace Makara;
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
        
        // Register services for DI
        builder.Services.AddSingleton<WordPickDatabase>();
        builder.Services.AddTransient<WordPickViewModel>();
        builder.Services.AddTransient<DataMigrateViewModel>(); // Register DataMigrateViewModel

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
