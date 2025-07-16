using System.Drawing;
using System.Drawing.Imaging;
using static Application.Commons.CustomShapes;


namespace Application.Commons
{
    public class ScreenCapturer
    {
        public Bitmap? CaptureWindow(IntPtr handle)
        {
            RECT rect;
            if (!WindowsAPI.GetWindowRect(handle, out rect))
            {
                return null;
            }

            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);


            using (var gfxBmp = Graphics.FromImage(bmp))
            {
                gfxBmp.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
            }

            return bmp;

        }
    }
}
