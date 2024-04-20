using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DMCockpit.Services
{
    public interface IDisplayManager
    {
        Task UpdateImageWithBase64(string base64String);
        void SetSubsection(int[] ints); // (x1, y1, x2, y2)

        string GetControlImage();

        event ImageUpdated ImageUpdatedEvent;
    }

    public delegate void ImageUpdated(string imageBase64);

    public class DisplayManager : IDisplayManager
    {
        public event ImageUpdated? ImageUpdatedEvent;

        private Bitmap image = new Bitmap(1, 1);
        private int[] ints = [0, 0,  160, 90];

        public async Task UpdateImageWithBase64(string base64String)
        {
            var bytes = Convert.FromBase64String(base64String);
            var content = new StreamContent(new MemoryStream(bytes));

            image = new Bitmap(await content.ReadAsStreamAsync());

            if (image.Height > image.Width)
            {
                image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            UpdateImage(image);
        }

        public string GetControlImage()
        {
            var bytes = BitmapToBytes(image);
            var base64String = Convert.ToBase64String(bytes);
            var imageSource = string.Format("data:image/jpeg;base64,{0}", base64String);

            return imageSource;
        }

        private void UpdateImage(Bitmap image)
        {
            image = DrawSubsection(image);
            var bytes = BitmapToBytes(image);
            var base64String = Convert.ToBase64String(bytes);
            var imageSource = string.Format("data:image/jpeg;base64,{0}", base64String);
            OnImageUpdated(imageSource);
        }

        private Bitmap DrawSubsection(Bitmap image)
        {
            var x1 = ints[0];
            var y1 = ints[1];
            var x2 = ints[2];
            var y2 = ints[3];

            var width = x2 - x1;
            var height = y2 - y1;

            var subsection = image.Clone(new Rectangle(x1, y1, width, height), image.PixelFormat);

            return subsection;
        }

        private byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public void SetSubsection(int[] ints) => this.ints = ints;
        protected virtual void OnImageUpdated(string imageBase64) => ImageUpdatedEvent?.Invoke(imageBase64);

    }
}
