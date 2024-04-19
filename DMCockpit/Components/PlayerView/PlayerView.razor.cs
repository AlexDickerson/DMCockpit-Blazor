using DMCockpit.Services;
using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace DMCockpit.Components.PlayerView
{
    public partial class PlayerView : ComponentBase
    {
        [Inject]
        private IDisplayManager DisplayManager { get; set; }

        private string imageBase64 = string.Empty;

        protected override void OnInitialized()
        {
            imageBase64 = DisplayManager.GetImagePath();

            DisplayManager.ImageUpdatedEvent += UpdateImage;
        }

        private void UpdateImage(Bitmap image)
        {
            var bytes = BitmapToBytes(image);
            var base64String = Convert.ToBase64String(bytes);
            var imageSource = string.Format("data:image/jpeg;base64,{0}", base64String);
            this.imageBase64 = imageSource;
            StateHasChanged();
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
