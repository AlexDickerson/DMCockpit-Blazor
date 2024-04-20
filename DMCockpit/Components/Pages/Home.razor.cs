using DMCockpit.MAUI_Pages;
using DMCockpit.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using Application = Microsoft.Maui.Controls.Application;

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
        private bool javascriptRegistered = false;

        private bool viewPortIsBeingDragged = false;

        protected override void OnInitialized()
        {
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await RegisterJavascript();
        }


        private async Task RegisterJavascript()
        {
            if (javascriptRegistered) return;

            try
            {
                string[] args = [".draggable", "controlMap"];
                await js.InvokeVoidAsync("dragAndDrop", args);

                args = ["mapViewPort", "controlMap"];
                await js.InvokeVoidAsync("resizeWithScroll", args);
                javascriptRegistered = true;
            }
            catch
            {
            }
        }

        private Coordinates[] previousPosition = new Coordinates[2];
        private async void UpdatePlayerWindow()
        {
            string[] args = ["mapViewPort", "controlMap"];
            var position = await js.InvokeAsync<Coordinates[]>("getPositionByID", args);

            if (previousPosition[0] != null && previousPosition[1] != null &&
                position[0].X == previousPosition[0].X && position[0].Y == previousPosition[0].Y &&
                position[1].X == previousPosition[1].X && position[1].Y == previousPosition[1].Y)
            {
                return;
            }

            DisplayManager.SetSubsection(position);

            previousPosition = position;
        }

        private void NewPlayerWindow()
        {
            var page = new PlayerViewPage();

            Window window = new(page);
            window.Title = "";
            window.X = 0;

            Application.Current.OpenWindow(window);

            playerWindow = window;
        }

        private async Task UploadFiles(IBrowserFile image)
        {
            await DisplayManager.UpdateImageWithBase64(image);

            this.imageBase64 = DisplayManager.GetControlImage();

            StateHasChanged();

            await js.InvokeVoidAsync("resizeImage", "controlMap");
        }

        private async Task OnViewPortMouseDown()
        {
            viewPortIsBeingDragged = true;
        }

        private async Task OnViewPortMouseUp()
        {
            viewPortIsBeingDragged = false;
        }

        private int frameSkip = 3;
        private int frame = 1;
        private async Task OnViewPortMouseMove()
        {
            if (viewPortIsBeingDragged && frame % frameSkip == 0)
            {
                UpdatePlayerWindow();
                frame = 1;
            }
            frame++;
        }
    }
}
