using DMCockpit_Library.Services;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Microsoft.UI.Xaml.Controls;

namespace DMCockpit.XAML_Pages;

public partial class DndBeyondBrowser : ContentPage
{
    private IHTTPInterceptor httpInterceptor;

    public DndBeyondBrowser(IHTTPInterceptor httpInterceptor)
    {
        InitializeComponent();

        this.httpInterceptor = httpInterceptor;

        dndBeyondBrowser.Navigated += Navigated;
    }

    private async void Navigated(object sender, WebNavigatedEventArgs e)
    {
        httpInterceptor.IntakeDNDBeyondRequest(sender as WebView, e.Url);
    }
}