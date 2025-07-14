using Application.Commons;
using System.Drawing;
using System.Text.RegularExpressions;


namespace Application.UseCases.Screenshot
{
    public static class ScreenshotOcrHandler
    {
        private static string GetDomainFromFirefoxScreenshot(string imagePath)
        {
            try
            {
                using (var img = new Bitmap(imagePath))
                {
                    // Ajusta estos valores según la resolución y layout de Firefox
                    Rectangle addressBarArea = new Rectangle(200, 60, 800, 50); // x, y, width, height

                    using (var croppedImg = img.Clone(addressBarArea, img.PixelFormat))
                    {
                        var ocrProcessor = new OcrProcessor();
                        string extractedText = ocrProcessor.RunOcr(croppedImg);

                        if (string.IsNullOrWhiteSpace(extractedText))
                            return "La barra de direcciones está vacía";

                        // Extraer dominio con regex
                        var match = Regex.Match(extractedText, @"(https?:\/\/)?(www\.)?([a-zA-Z0-9.-]+\.[a-zA-Z]{2,})");
                        if (match.Success)
                        {
                            string domain = match.Groups[3].Value;
                            return $"Dominio detectado: {domain}";
                        }
                        else
                        {
                            return "No se detectó ningún dominio en la barra de direcciones";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public static string Execute(string processName, string savePath)
        {
            var watcher = new WindowWatcher(processName);
            var windowHandle = watcher.WaitForActiveWindow();
            if (windowHandle != nint.Zero)
            {
                var capturer = new ScreenCapturer();
                var capturedSuccessfully = capturer.CaptureWindow(windowHandle, savePath);

                string result = GetDomainFromFirefoxScreenshot(savePath);

                return result;
            }

            return string.Empty;
        }
    }
}
