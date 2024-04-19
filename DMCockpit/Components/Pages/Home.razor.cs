using DMCockpit.MAUI_Pages;
using DMCockpit.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Maui.Storage;

namespace DMCockpit.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; }

        Window? playerWindow = null;

        protected override void OnInitialized()
        {
        }

        private void NewPlayerWindow()
        {
            var page = new PlayerViewPage();

            Window window = new(page);
            window.Title = "";
       
            Application.Current.OpenWindow(window);

            playerWindow = window;
        }

        private async Task UploadFiles(IBrowserFile image)
        {
            byte[] bytes = new byte[image.Size];
            await image.OpenReadStream(maxAllowedSize: image.Size).ReadAsync(bytes);
            string base64String = Convert.ToBase64String(bytes);

            await DisplayManager.UpdateImageWithBase64(base64String);
        }
    }
}
