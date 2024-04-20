using DMCockpit.Services;
using Microsoft.AspNetCore.Components;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace DMCockpit.Components.PlayerView
{
    public partial class PlayerView : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; }

        private string imageBase64 = string.Empty;
        private string overlayGridImageBase64 = string.Empty;

        private int displayDiagonalInches = 22;
        private int displayResolutionX = 1920;
        private int displayResolutionY = 1080;
        private bool gridVisible = true;
        private int gridThickness = 3;

        protected override void OnInitialized()
        {
            DisplayManager.ImageUpdatedEvent += UpdateImage;
            overlayGridImageBase64 = CreateOverlayGridImage();
        }

        private void UpdateImage(string imageBase64)
        {
            this.imageBase64 = imageBase64;
            StateHasChanged();
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
                using (Pen pen = new Pen(System.Drawing.Color.Black, gridThickness))
                {
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
            }

            var bytes = BitmapToBytes(overlayImage);
            var base64String = Convert.ToBase64String(bytes);
            var imageSource = string.Format("data:image/jpeg;base64,{0}", base64String);

            return imageSource;
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
