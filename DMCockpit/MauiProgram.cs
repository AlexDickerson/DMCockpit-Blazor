using DMCockpit.Services;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace DMCockpit
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

            builder.Services.RegisterDMCockpitServices();
            builder.Services.AddMudServices();

            return builder.Build();
        }

        private static void RegisterDMCockpitServices(this IServiceCollection services)
        {
            services.AddSingleton<IDisplayManager, DisplayManager>();
        }
    }
}
