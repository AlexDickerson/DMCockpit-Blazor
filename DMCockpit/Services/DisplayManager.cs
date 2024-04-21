using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;

namespace DMCockpit.Services
{
    public interface IDisplayManager
    {
        Task UpdateImageWithBase64(IBrowserFile file);
        void SetSubsection(Coordinates[] coordinates);

        string GetControlImage();

        event ImageUpdated ImageUpdatedEvent;
        event CoordinatesUpdated CoordiantesUpdatedEvent;
    }

    public class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public delegate void ImageUpdated(string base64Image);
    public delegate void CoordinatesUpdated(Coordinates[] coordinates);

    public class DisplayManager : IDisplayManager
    {
        public event ImageUpdated? ImageUpdatedEvent;
        public event CoordinatesUpdated? CoordiantesUpdatedEvent;

        private Image image;

        public async Task UpdateImageWithBase64(IBrowserFile file)
        {
            using (var stream = file.OpenReadStream(maxAllowedSize: file.Size))
            {
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using (var content = new StreamContent(memoryStream))
                {
                    image = SixLabors.ImageSharp.Image.Load(await content.ReadAsStreamAsync());
                }
            }

            if (image.Height > image.Width)
            {
                image.Mutate(x => x.RotateFlip(RotateMode.Rotate90, FlipMode.None));
            }

            UpdateImage(image);
        }

        public string GetControlImage()
        {
            return image.ToBase64String(PngFormat.Instance);
        }

        private void UpdateCoordinates(Coordinates[] coordinates)
        {
            OnCoordinatesUpdated(coordinates);
        }

        private void UpdateImage(Image image)
        {
            OnImageUpdated(image.ToBase64String(PngFormat.Instance));
        }

        private Image DrawSubsection(Image image, Coordinates[] coordinates)
        {
            if (coordinates[0] == null || coordinates[1] == null)
            {
                return image;
            }

            var x1 = (int)coordinates[0].X;
            var y1 = (int)coordinates[0].Y;
            var x2 = (int)coordinates[1].X;
            var y2 = (int)coordinates[1].Y;

            x1 = (int)(x1 * image.Width / 100);
            y1 = (int)(y1 * image.Height / 100);
            x2 = (int)(x2 * image.Width / 100);
            y2 = (int)(y2 * image.Height / 100);

            // ensure coordinates are within bounds
            x1 = Math.Max(0, x1);
            y1 = Math.Max(0, y1);
            x2 = Math.Min(image.Width, x2);
            y2 = Math.Min(image.Height, y2);

            var width = x2 - x1;
            var height = y2 - y1;

            // ensure ratio is always 16:9
            if (width > height)
            {
                height = (int)(width * 9 / 16);
            }
            else
            {
                width = (int)(height * 16 / 9);
            }

            //Bitmap.Clone with throw an OutOfMemoryException if the rectangle is outside the bounds of the image
            //So we need to ensure the rectangle is within the bounds of the image
            if (y1 + height > image.Height)
            {
                y1 = image.Height - height;
            }

            if (x1 + width > image.Width)
            {
                x1 = image.Width - width;
            }

            //var subsection = image.Clone(new Rectangle(x1, y1, width, height));

            var subsction = image.Clone(x => x.Crop(new Rectangle(x1, y1, width, height)));

            return subsction;
        }

        public void SetSubsection(Coordinates[] coordinates)
        {
            UpdateCoordinates(coordinates);
        }

        protected virtual void OnCoordinatesUpdated(Coordinates[] coordinates) => CoordiantesUpdatedEvent?.Invoke(coordinates);
        protected virtual void OnImageUpdated(string base64Image) => ImageUpdatedEvent?.Invoke(base64Image);

    }
}
