using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace DMCockpit.Services
{
    public interface IDisplayManager
    {
        Task UpdateImageWithBase64(string base64String);

        string GetImagePath();

        event ImageUpdated ImageUpdatedEvent;
    }

    public delegate void ImageUpdated(Bitmap image);

    public class DisplayManager : IDisplayManager
    {
        public event ImageUpdated ImageUpdatedEvent;

        private string imagePath = string.Empty;
        private Bitmap image;

        public DisplayManager()
        {
        }

        public string GetImagePath()
        {
            return imagePath;
        }

        public async Task UpdateImageWithBase64(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);
            var content = new StreamContent(new MemoryStream(bytes));

            image = new Bitmap(await content.ReadAsStreamAsync());

            if (image.Height > image.Width)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            OnImageUpdated(image);
        }

        protected virtual void OnImageUpdated(Bitmap image)
        {
            ImageUpdatedEvent?.Invoke(image);
        }
    }
}
