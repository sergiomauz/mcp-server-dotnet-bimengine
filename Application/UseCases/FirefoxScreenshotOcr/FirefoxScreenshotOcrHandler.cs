using System.Drawing;
using System.Drawing.Imaging;
using Application.Commons;


namespace Application.UseCases.FirefoxScreenshotOcr
{
    public static class FirefoxScreenshotOcrHandler
    {
        private static string TryToGetDomainFromFirefoxScreenshot(string tessDataPath, Bitmap capturedBitmap, out Bitmap? croppedBitmap)
        {
            try
            {
                // Set values considering resolution
                // x, y, width, height
                var addressBarArea = new Rectangle(200, 60, 800, 50);
                croppedBitmap = capturedBitmap.Clone(addressBarArea, capturedBitmap.PixelFormat);

                // Preprocess nad improve OCR and get domain
                var ocrProcessor = new OcrProcessor();
                var preprocessedImg = ocrProcessor.PreprocessImageGrayBW(croppedBitmap);
                var extractedText = ocrProcessor.RunOcr(tessDataPath, preprocessedImg);
                if (string.IsNullOrWhiteSpace(extractedText))
                {
                    croppedBitmap = null;
                    return "No characters were detected.";
                }

                //
                return $"Detected:\n\n\n=============\n{extractedText}\n=============\n\n\n";
            }
            catch (Exception ex)
            {
                croppedBitmap = null;
                return $"Error processing image: {ex.Message}";
            }
        }

        public static string Execute(string processName, string tessDataPath, string screenshotPath, string croppedImagePath)
        {
            var watcher = new WindowWatcher(processName);
            var windowHandle = watcher.WaitForActiveWindow();
            if (windowHandle != nint.Zero && windowHandle != nint.MinValue)
            {
                var capturer = new ScreenCapturer();
                using (var capturedBitmap = capturer.CaptureWindow(windowHandle))
                {
                    var result = TryToGetDomainFromFirefoxScreenshot(tessDataPath, capturedBitmap, out Bitmap? croppedBitmap);
                    capturedBitmap.Save(screenshotPath, ImageFormat.Png);
                    if (croppedBitmap != null)
                    {
                        croppedBitmap.Save(croppedImagePath, ImageFormat.Png);
                        croppedBitmap.Dispose();
                    }

                    return result;
                }
            }

            return "No Firefox window was detected.";
        }
    }
}
