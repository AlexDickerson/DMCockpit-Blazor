﻿using DMCockpit.XAML_Pages;
using DMCockpit.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Application = Microsoft.Maui.Controls.Application;
using DMCockpit_Library.Javascript_Interop;

namespace DMCockpit.Components.Pages
{
    public partial class Control : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; } = null!;

        [Inject]
        private IDMCockpitInterop DMCockpitInterop { get; set; } = null!;

        Window? playerWindow = null;
        private string imageBase64 = string.Empty;
        private bool javascriptRegistered = false;

        private bool viewPortIsBeingDragged = false;
        private bool[]? maskBitmap;

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
                await DMCockpitInterop.MakeElementDraggable(".draggable", "controlMap");
                await DMCockpitInterop.ResizeWithScroll("mapViewPort", "controlMap");   
                await DMCockpitInterop.CreateDrawableCanvas("maskCanvas", "controlMap", "mapViewPort");

                javascriptRegistered = true;
            }
            catch
            {
            }
        }

        private async Task UpdatePlayerWindow()
        {
            var position = await DMCockpitInterop.GetViewPortPositionOnMap("mapViewPort", "controlMap");
            var maskBitmapTemp = await DMCockpitInterop.GetCanvasBitmap("maskCanvas");
            if (maskBitmapTemp.Length > 0)
            {
                maskBitmap = maskBitmapTemp;
                DisplayManager.SetMask(maskBitmap);
            }

            DisplayManager.SetSubsection(position);
        }

        private void NewPlayerWindow()
        {
            var page = new PlayerViewPage();

            Window window = new(page)
            {
                Title = "PlayerView",
                X = 2000
            };

            var currentApp = Application.Current ?? throw new NullReferenceException("Application.Current is null. How?");
            currentApp.OpenWindow(window);

            playerWindow = window;
        }

        private async Task UploadFiles(IBrowserFile image)
        {
            await DisplayManager.UpdateImageWithBase64(image);

            this.imageBase64 = DisplayManager.GetControlImage();

            StateHasChanged();

            await DMCockpitInterop.ResizeImage("controlMap");
        }

        private async Task OnCanvasMouseUp()
        {
            await UpdatePlayerWindow();
        }

        private void OnViewPortMouseDown(MouseEventArgs e)
        {
            if (e.ShiftKey)
            {
                viewPortIsBeingDragged = true;
            }
        }

        private async Task OnViewPortMouseUp(MouseEventArgs e)
        {
            viewPortIsBeingDragged = false;
            await UpdatePlayerWindow();
        }

        private void OnViewPortMouseLeave(MouseEventArgs e)
        {
            viewPortIsBeingDragged = false;
        }

        private readonly int frameSkip = 3;
        private int frame = 1;
        private async Task OnViewPortMouseMove()
        {
            if (viewPortIsBeingDragged && frame % frameSkip == 0)
            {
                await UpdatePlayerWindow();
                frame = 1;
            }
            frame++;
        }
    }
}
