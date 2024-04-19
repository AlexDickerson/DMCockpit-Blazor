using DMCockpit.MAUI_Pages;
using Microsoft.AspNetCore.Components;

namespace DMCockpit.Components.Pages
{
    public partial class Home : ComponentBase
    {
        private void NewWindow()
        {
            var page = new PlayerViewPage();

            Window window = new(page);
            window.Title = "";


            Application.Current.OpenWindow(window);
        }
    }
}
