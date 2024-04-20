using DMCockpit.MAUI_Pages;
using DMCockpit.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Maui.Storage;
using Microsoft.JSInterop;

namespace DMCockpit.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; }

        [Inject]
        private IJSRuntime js { get; set; }

        Window? playerWindow = null;
        private string imageBase64 = string.Empty;

        protected override void OnInitialized()
        {
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await js.InvokeVoidAsync("dragAndDrop", ".draggable");
            await js.InvokeVoidAsync("resizeWithScroll", "mapViewPort");
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

            this.imageBase64 = DisplayManager.GetControlImage();
        }
    }
}
