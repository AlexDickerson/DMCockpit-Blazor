using DMCockpit_Library.Extensions;
using DMCockpit_Library.Managers;
using DMCockpit_Library.Relays;

namespace DMCockpit.XAML_Pages;

public partial class DndBeyondBrowser : ContentPage
{
    private DndBeyondBrowserRelay webViewInterceptor;
    private ISettingsManager settingsManager;
    private string jsFileText;
    private bool jsLoaded = false;

    public DndBeyondBrowser(DndBeyondBrowserRelay webViewInterceptor, ISettingsManager settingsManager)
    {
        InitializeComponent();

        this.webViewInterceptor = webViewInterceptor;
        this.settingsManager = settingsManager;

        dndBeyondBrowser.Navigated += Navigated;

        var currentDir = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(currentDir, "wwwroot/JSFolder/DMCockpit.js");
        var jsFileText = File.ReadAllText(filePath);

        this.jsFileText = jsFileText.MinifyJS();
    }

    private async void Navigated(object sender, WebNavigatedEventArgs e)
    {
        WebView webView = sender as WebView;

        await webView.EvaluateJavaScriptAsync(jsFileText);

        webViewInterceptor.IntakeDNDBeyondRequest(sender as WebView, e.Url);
    }

    private void ClickGestureRecognizer_Clicked(object sender, PointerEventArgs e)
    {
        Console.WriteLine("test");
    }

    public void DndBeyondBrowserUnloaded(object sender, EventArgs e)
    {
        settingsManager.SaveWindowLocation("DndBeyondBrowser", (int)this.Window.X, (int)this.Window.Y);
    }
}