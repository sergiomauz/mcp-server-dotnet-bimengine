using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Application.Commons;


namespace Application.UseCases.Screenshot
{
    public static class ScreenshotOcrHandler
    {
        private static Bitmap PreprocessImage(Bitmap original)
        {
            // Convertir a 24bpp para evitar problemas de stride y PixelFormat
            var sourceBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);
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

                    byte blue = pixelBuffer[idx];
                    byte green = pixelBuffer[idx + 1];
                    byte red = pixelBuffer[idx + 2];

                    // Gray scale
                    byte gray = (byte)(0.299 * red + 0.587 * green + 0.114 * blue);

                    // To binary
                    byte binarized = (gray > 150) ? (byte)255 : (byte)0;
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

        private static string GetDomainFromFirefoxScreenshot(string screenshotPath, string croppedImagePath)
        {
            try
            {
                using (var img = new Bitmap(screenshotPath))
                {
                    // Set values considering resolution
                    // x, y, width, height
                    var addressBarArea = new Rectangle(200, 60, 800, 50);

                    using (var croppedImg = img.Clone(addressBarArea, img.PixelFormat))
                    {
                        // Guarda el recorte original si quieres verificar
                        croppedImg.Save(croppedImagePath, ImageFormat.Png);

                        // Preprocesar para mejorar OCR
                        var preprocessedImg = PreprocessImage(croppedImg);
                        var ocrProcessor = new OcrProcessor();
                        var extractedText = ocrProcessor.RunOcr(preprocessedImg);

                        if (string.IsNullOrWhiteSpace(extractedText))
                            return "No characters were detected.";

                        // Clean and get first line
                        extractedText = extractedText.Trim().Split(' ')[0].Split('\n')[0].Split('\n')[0];

                        return extractedText;
                    }
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
                var capturedSuccessfully = capturer.CaptureWindow(windowHandle, screenshotPath);
                var result = GetDomainFromFirefoxScreenshot(screenshotPath, croppedImagePath);

                return result;
            }

            return string.Empty;
        }
    }
}
