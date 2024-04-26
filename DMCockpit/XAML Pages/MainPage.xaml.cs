using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System.Net;

namespace DMCockpit
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            blazorWebView.BlazorWebViewInitialized += BlazorWebViewInitalized;
        }

        private void AppUnloaded(object sender, EventArgs e)
        {
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
