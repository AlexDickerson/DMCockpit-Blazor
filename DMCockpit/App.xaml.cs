using DMCockpit.XAML_Pages;
using Application = Microsoft.Maui.Controls.Application;

namespace DMCockpit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var openSpotify = Environment.GetEnvironmentVariable("DMCOCKPIT_OPEN_SPOTIFY") ?? "true";

            // I don't understand why this works, but opening Spotify in a WebView seems to set a cookie or something somewhere that lets the embedded Spotify iFrames autheticate properly
            // So we open Spotify, let the user log in, and then close the app and open it again without opening Spotify
            // Looks like these tokens last for a year, so shouldn't need to do this often
            if (openSpotify == "true")
            {
                MainPage = new Spotify();
            }
            else
            {
                MainPage = new MainPage();
            }
        }
    }
}
