using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Web.WebView2.Core;
using System.Net;

namespace DMCockpit
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AppUnloaded(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}
