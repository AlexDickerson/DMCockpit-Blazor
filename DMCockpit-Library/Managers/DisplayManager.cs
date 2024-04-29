using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace DMCockpit_Library.Managers
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

        public void SetSubsection(Coordinates[] coordinates) => UpdateCoordinates(coordinates);
        public void SetMask(bool[] mask) => DrawMask(mask);
        public string GetControlImage() => image != null ? image.ToBase64String(PngFormat.Instance) : string.Empty;

        public async Task UpdateImageWithBase64(IBrowserFile file)
        {
            var image = await ReadImageFromBrowserFile(file);

            if (image.Height > image.Width) image.Mutate(x => x.RotateFlip(RotateMode.Rotate90, FlipMode.None));
            if (bitmaskImage == null) DrawMask(new bool[image.Width * image.Height]);

            UpdateImage(image);
        }

        private async Task<Image> ReadImageFromBrowserFile(IBrowserFile file)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: file.Size);

            var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            using var content = new StreamContent(memoryStream);
            return SixLabors.ImageSharp.Image.Load(await content.ReadAsStreamAsync());
        }

        private void DrawMask(bool[] mask)
        {
            if (image == null)  return;

            var maskTotalPixels = mask.Length;

            var bytes = CreateMaskByteArray(mask, maskTotalPixels);

            var maskImage = CreatMaskImage(bytes, maskTotalPixels);

            OnMaskUpdated(maskImage.ToBase64String(PngFormat.Instance));

            bitmaskImage = maskImage;
        }

        private Image CreatMaskImage(byte[] bytes, int maskLength)
        {
            var imageAspectRatio = (double) image.Width / image.Height;
            var maskWidth = (int)Math.Sqrt(maskLength * imageAspectRatio);
            var maskHeight = maskLength / maskWidth;

            var maskImage = Image.LoadPixelData<A8>(bytes, maskWidth, maskHeight);

            maskImage.Mutate(x => x.Resize(image.Width, image.Height).GaussianBlur(1));

            return maskImage;
        }

        private byte[] CreateMaskByteArray(bool[] mask, int maskLength)
        {
            byte[] bytes = new byte[maskLength];
            for (int i = 0; i < maskLength; i++)
            {
                bytes[i] = mask[i] ? (byte)0 : (byte)255;
            }

            return bytes;
        }

        protected virtual void OnCoordinatesUpdated(Coordinates[] coordinates) => CoordiantesUpdatedEvent?.Invoke(coordinates);
        protected virtual void OnImageUpdated(string base64Image) => ImageUpdatedEvent?.Invoke(base64Image);
        protected virtual void OnMaskUpdated(string base64Mask) => MaskUpdatedEvent?.Invoke(base64Mask);

        private void UpdateCoordinates(Coordinates[] coordinates) => OnCoordinatesUpdated(coordinates);
        private void UpdateImage(Image image) => OnImageUpdated(image.ToBase64String(PngFormat.Instance));
    }
}
