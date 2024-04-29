using DMCockpit_Library.Managers;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Web.WebView2.Core;

namespace DMCockpit
{
    public partial class MainPage : ContentPage
    {
        private readonly ISettingsManager settingsManager;

        public MainPage(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;

            InitializeComponent();

            blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitalized;
        }

        private void AppUnloaded(object sender, EventArgs e)
        {
            settingsManager.SaveSettings();

            Application.Current.Quit();
        }

        private async void BlazorWebViewInitalized(object sender, BlazorWebViewInitializedEventArgs e)
        {
            e.WebView.CoreWebView2.FrameNavigationStarting += FrameNavigationStarting;
        }

        private async void FrameNavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs e)
        {
            e.AdditionalAllowedFrameAncestors = "* https://www.dndbeyond.com https://dndbeyond.com";
        }
    }
}
