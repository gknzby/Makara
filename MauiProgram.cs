using Microsoft.Extensions.Logging;
using Makara.Data;
using Makara.ViewModels;
using Makara.Views;

namespace Makara;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder _builder = MauiApp.CreateBuilder();
        _builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        //// Register services for DI
        //builder.Services.AddSingleton<WordPickDatabase>();
        //builder.Services.AddTransient<WordPickViewModel>();
        //builder.Services.AddTransient<DataMigrateViewModel>();

        // Register only the behavior manager service
        _builder.Services.AddSingleton<IBehaviorManager, BehaviorManager>();

        // Register view models
        _builder.Services.AddSingleton<ItemsViewModel>();

#if DEBUG
        _builder.Logging.AddDebug();
#endif

        return _builder.Build();
    }
}
