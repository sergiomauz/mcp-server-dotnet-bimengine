using System.Drawing;
using System.Drawing.Imaging;
using static Application.Commons.CustomShapes;


namespace Application.Commons
{
    public class ScreenCapturer
    {
        public bool CaptureWindow(IntPtr handle, string savePath)
        {
            RECT rect;
            bool success = false;
            if (!WindowsAPI.GetWindowRect(handle, out rect))
            {
                return success;
            }

            var width = rect.Right - rect.Left;
            var height = rect.Bottom - rect.Top;
            using (var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                using (var gfxBmp = Graphics.FromImage(bmp))
                {
                    gfxBmp.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height), CopyPixelOperation.SourceCopy);
                }

                bmp.Save(savePath, ImageFormat.Png);
                success = true;
            }

            return success;
        }
    }
}
