using DMCockpit.Services;
using DMCockpit_Library;
using DMCockpit_Library.Managers;
using DMCockpit_Library.Relays;
using DMCockpit_Library.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using MudBlazor.Services;

namespace DMCockpit
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--disable-web-security --user-data-dir=~/chromeTemp");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.ConfigureLifecycleEvents(events =>
            {
                // Make sure to add "using Microsoft.Maui.LifecycleEvents;" in the top of the file 
                events.AddWindows(windowsLifecycleBuilder =>
                {
                    windowsLifecycleBuilder.OnWindowCreated(window =>
                    {
                        if (window.Title != "PlayerView") return;
                        var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
                        switch (appWindow.Presenter)
                        {
                            case Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter:
                                overlappedPresenter.SetBorderAndTitleBar(false, false);
                                overlappedPresenter.Maximize();
                                break;
                        }
                    });
                });
            });

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
            services.AddTransient<IDMCockpitJSInterop, DMCockpitJSInterop>();
            services.AddSingleton<IMiniTracking, MiniTracking>();
            services.AddSingleton<IHotKeyObersevable, HotKeyHandler>();
            services.AddSingleton<ISettingsManager, SettingsManager>();
            services.AddSingleton<DndBeyondBrowserRelay, WebviewInterceptor>();
            services.AddSingleton<IHTTPRelay, DMCockpitHTTPListener>();
            services.AddSingleton<IDMCockpitConfigurationService, DMCockpitConfigurationService>();
        }
    }
}
