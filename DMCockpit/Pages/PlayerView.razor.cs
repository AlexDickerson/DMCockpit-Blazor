﻿using Microsoft.AspNetCore.Components;
using System.Drawing.Drawing2D;
using System.Drawing;
using Microsoft.JSInterop;
using DMCockpit_Library.Managers;

namespace DMCockpit.Pages
{
    public partial class PlayerView : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; } = default!;

        [Inject]
        private IJSRuntime Js { get; set; } = default!;

        private string imageBase64 = string.Empty;
        private string overlayGridImageBase64 = string.Empty;
        private string maskBase64 = string.Empty;

        private bool mapFirstRender = true;

        private readonly int displayDiagonalInches = 22;
        private readonly int displayResolutionX = 1920;
        private readonly int displayResolutionY = 1080;
        private readonly bool gridVisible = true;
        private readonly int gridThickness = 3;

        private readonly Coordinates[] coordinates = new Coordinates[2];

        protected override void OnInitialized()
        {
            DisplayManager.ImageUpdatedEvent += UpdateImage;
            DisplayManager.CoordiantesUpdatedEvent += UpdateCoordinates;
            DisplayManager.MaskUpdatedEvent += UpdateMask;
            overlayGridImageBase64 = CreateOverlayGridImage();

            coordinates[0] = new Coordinates { X = 0, Y = 0 };
            coordinates[1] = new Coordinates { X = 1600, Y = 900 };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                return;
            }

            if (mapFirstRender)
            {
                await DrawImage(coordinates);
                mapFirstRender = false;

                string[] args = ["maskCanvas", "playerMapCanvas"];
                await Js.InvokeVoidAsync("overlayMaskCanvas", args);
            }
        }

        private void UpdateImage(string imageBase64)
        {
            this.imageBase64 = imageBase64;
            StateHasChanged();
        }

        private void UpdateMask(string maskBase64)
        {
            this.maskBase64 = maskBase64;
            StateHasChanged();
        }

        private async void UpdateCoordinates(Coordinates[] coordinates)
        {
            await DrawImage(coordinates);
        }

        private async Task DrawImage(Coordinates[] coordinates)
        {
            string[] args = ["playerMap", "playerMapCanvas", coordinates[0].X.ToString(), coordinates[0].Y.ToString(), coordinates[1].X.ToString(), coordinates[1].Y.ToString()];
            await Js.InvokeVoidAsync("drawImageWithCoordinates", args);

            string[] maskArgs = ["maskImage", "maskCanvas", coordinates[0].X.ToString(), coordinates[0].Y.ToString(), coordinates[1].X.ToString(), coordinates[1].Y.ToString()];
            await Js.InvokeVoidAsync("drawImageWithCoordinates", maskArgs);
        }

        private string CreateOverlayGridImage()
        {
            var overlayImage = new Bitmap(displayResolutionX, displayResolutionY);
            int ppi = (int)Math.Sqrt(Math.Pow(displayResolutionX, 2) + Math.Pow(displayResolutionY, 2)) / displayDiagonalInches;
            var verticalInches = displayResolutionY / ppi;
            var horizontalInches = displayResolutionX / ppi;
            using (Graphics g = Graphics.FromImage(overlayImage))
            {
                g.Clear(System.Drawing.Color.Transparent);
                using Pen pen = new(System.Drawing.Color.Black, gridThickness);
                pen.DashStyle = DashStyle.Dash;
                for (int i = 1; i < verticalInches; i++)
                {
                    g.DrawLine(pen, 0, i * ppi, displayResolutionX, i * ppi);
                }
                for (int i = 1; i < horizontalInches; i++)
                {
                    g.DrawLine(pen, i * ppi, 0, i * ppi, displayResolutionY);
                }
            }

            var bytes = BitmapToBytes(overlayImage);
            var base64String = Convert.ToBase64String(bytes);
            var imageSource = string.Format("data:image/jpeg;base64,{0}", base64String);

            return imageSource;
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using MemoryStream stream = new();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
