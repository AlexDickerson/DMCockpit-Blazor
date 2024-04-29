using Emgu.CV;
using Emgu.CV.CvEnum;

namespace DMCockpit_Library
{
    public interface IMiniTracking
    {
        void InitiateTracking();
    }

    public class MiniTracking : IMiniTracking
    {
        public void InitiateTracking()
        {
            var capture = new VideoCapture(0);

            capture.Set(CapProp.FrameWidth, 1920);
            capture.Set(CapProp.FrameHeight, 1080);
            capture.Set(CapProp.Autofocus, 0);

            while (true)
            {
                capture.Start();

                Mat output = new();
                capture.Read(output);

                CvInvoke.Imshow("Test", output);
            }
        }
    }
}
