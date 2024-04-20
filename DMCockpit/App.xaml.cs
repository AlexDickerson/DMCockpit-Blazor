using Application = Microsoft.Maui.Controls.Application;

namespace DMCockpit
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
    }
}
