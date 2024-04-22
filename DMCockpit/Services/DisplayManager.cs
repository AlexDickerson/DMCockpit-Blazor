using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace DMCockpit.Services
{
    public interface IDisplayManager
    {
        Task UpdateImageWithBase64(IBrowserFile file);
        void SetSubsection(Coordinates[] coordinates);
        void SetMask(bool[] mask);

        string GetControlImage();

        event ImageUpdated ImageUpdatedEvent;
        event CoordinatesUpdated CoordiantesUpdatedEvent;
        event MaskUpdated MaskUpdatedEvent;
    }

    public class Coordinates
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public delegate void ImageUpdated(string base64Image);
    public delegate void MaskUpdated(string base64Mask);
    public delegate void CoordinatesUpdated(Coordinates[] coordinates);

    public class DisplayManager : IDisplayManager
    {
        public event ImageUpdated? ImageUpdatedEvent;
        public event CoordinatesUpdated? CoordiantesUpdatedEvent;
        public event MaskUpdated? MaskUpdatedEvent;

        private Image? image = null;
        private Image? bitmaskImage = null;

        public async Task UpdateImageWithBase64(IBrowserFile file)
        {
            using (var stream = file.OpenReadStream(maxAllowedSize: file.Size))
            {
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                using var content = new StreamContent(memoryStream);
                image = SixLabors.ImageSharp.Image.Load(await content.ReadAsStreamAsync());
            }

            if (image.Height > image.Width)
            {
                image.Mutate(x => x.RotateFlip(RotateMode.Rotate90, FlipMode.None));
            }

            if (bitmaskImage == null)
            {
                bool[] bools = new bool[image.Width * image.Height];
                DrawMask(bools);
            }

            UpdateImage(image);
        }

        public string GetControlImage()
        {
            if (image == null)
            {
                return string.Empty;
            }

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

        private void DrawMask(bool[] mask)
        {
            if (image == null)
            {
                return;
            }

            byte[] bytes = new byte[mask.Length];
            for (int i = 0; i < mask.Length; i++)
            {
                bytes[i] = mask[i] ? (byte)0 : (byte)255;
            }

            var imageAspectRatio = (double)image.Width / image.Height;

            var maskTotalPixels = mask.Length;

            var maskWidth = (int)Math.Sqrt(maskTotalPixels * imageAspectRatio);
            var maskHeight = (int)(maskTotalPixels / maskWidth);

            var maskImage = Image.LoadPixelData<A8>(bytes, maskWidth, maskHeight);

            maskImage.Mutate(x => x.Resize(image.Width, image.Height).GaussianBlur(1));

            OnMaskUpdated(maskImage.ToBase64String(PngFormat.Instance));

            bitmaskImage = maskImage;
        }

        public void SetSubsection(Coordinates[] coordinates)
        {
            UpdateCoordinates(coordinates);
        }

        public void SetMask(bool[] mask)
        {
            DrawMask(mask);
        }

        protected virtual void OnCoordinatesUpdated(Coordinates[] coordinates) => CoordiantesUpdatedEvent?.Invoke(coordinates);
        protected virtual void OnImageUpdated(string base64Image) => ImageUpdatedEvent?.Invoke(base64Image);
        protected virtual void OnMaskUpdated(string base64Mask) => MaskUpdatedEvent?.Invoke(base64Mask);


    }
}
