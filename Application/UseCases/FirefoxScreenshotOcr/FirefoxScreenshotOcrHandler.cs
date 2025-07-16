using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Application.Commons;


namespace Application.UseCases.FirefoxScreenshotOcr
{
    public static class FirefoxScreenshotOcrHandler
    {
        private static Bitmap _preprocessImage(Bitmap original)
        {
            // Convert to 24bpp to avoid stride and PixelFormat issues
            var sourceBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format64bppPArgb);
            using (var g = Graphics.FromImage(sourceBitmap))
            {
                g.DrawImage(original, 0, 0, original.Width, original.Height);
            }

            var resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height, PixelFormat.Format24bppRgb);
            var rect = new Rectangle(0, 0, sourceBitmap.Width, sourceBitmap.Height);
            var sourceData = sourceBitmap.LockBits(rect, ImageLockMode.ReadOnly, sourceBitmap.PixelFormat);
            var resultData = resultBitmap.LockBits(rect, ImageLockMode.WriteOnly, resultBitmap.PixelFormat);
            var bytesPerPixel = Image.GetPixelFormatSize(sourceBitmap.PixelFormat) / 8;
            var height = sourceBitmap.Height;
            var stride = sourceData.Stride;

            var pixelBuffer = new byte[stride * height];
            var resultBuffer = new byte[stride * height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            for (int y = 0; y < height; y++)
            {
                int rowOffset = y * stride;

                for (int x = 0; x < sourceBitmap.Width; x++)
                {
                    int idx = rowOffset + x * bytesPerPixel;

                    var blue = pixelBuffer[idx];
                    var green = pixelBuffer[idx + 1];
                    var red = pixelBuffer[idx + 2];

                    // Gray scale
                    var gray = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);

                    // To binary
                    var binarized = (gray > 150) ? (byte)255 : (byte)0;
                    resultBuffer[idx] = binarized;
                    resultBuffer[idx + 1] = binarized;
                    resultBuffer[idx + 2] = binarized;
                }
            }
            Marshal.Copy(resultBuffer, 0, resultData.Scan0, resultBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            resultBitmap.UnlockBits(resultData);
            sourceBitmap.Dispose();

            return resultBitmap;
        }

        private static string _getDomainFromFirefoxScreenshot(Bitmap capturedBitmap, string croppedImagePath)
        {
            try
            {
                // Set values considering resolution
                // x, y, width, height
                var addressBarArea = new Rectangle(200, 60, 800, 50);
                using (var croppedImg = capturedBitmap.Clone(addressBarArea, capturedBitmap.PixelFormat))
                {
                    // Save cropped image
                    croppedImg.Save(croppedImagePath, ImageFormat.Png);

                    // Preprocess nad improve OCR and get domain
                    var preprocessedImg = _preprocessImage(croppedImg);
                    var ocrProcessor = new OcrProcessor();
                    var extractedText = ocrProcessor.RunOcr(preprocessedImg);
                    if (string.IsNullOrWhiteSpace(extractedText))
                        return "No characters were detected.";

                    // Clean and get first line. Domain always
                    // extractedText = extractedText.Trim().Split(' ')[0].Split('\n')[0].Split('\n')[0];

                    //
                    return $"Detected:\n\n\n=============\n{extractedText}\n=============\n\n\n";
                }
            }
            catch (Exception ex)
            {
                return $"Error processing image: {ex.Message}";
            }
        }

        public static string Execute(string processName, string screenshotPath, string croppedImagePath)
        {
            var watcher = new WindowWatcher(processName);
            var windowHandle = watcher.WaitForActiveWindow();
            if (windowHandle != nint.Zero)
            {
                var capturer = new ScreenCapturer();
                using (var capturedBitmap = capturer.CaptureWindow(windowHandle))
                {
                    var result = _getDomainFromFirefoxScreenshot(capturedBitmap, croppedImagePath);


                    capturedBitmap.Save(screenshotPath, ImageFormat.Png);

                    return result;
                }
            }

            return "No Firefox window was detected.";
        }
    }
}
