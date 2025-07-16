using System.ComponentModel;
using ModelContextProtocol.Server;
using Commons;
using Application.UseCases.FirefoxScreenshotOcr;


namespace RevitMcpServer.Tools
{
    [McpServerToolType]
    public static class BimEngineTools
    {
        [McpServerTool, Description("Echoes the message back to the client.")]
        public static string RevitScreenshotOcr(string message)
        {
            return $"Hello Revit!: {message}";
        }

        [McpServerTool, Description("Captures a screenshot of a Firefox window and tries to get domain from address bar. Wait for 10 attempts, then it fails.")]
        public static object FirefoxScreenshotOcr(string message)
        {
            // Process to detect
            var process = "firefox";

            // Path to save images
            var screenshotPath = $"{Constants.CAPTURES}\\{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var croppedImagePath = $"{Constants.CAPTURES}\\{process}_cropped_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            //
            var imageCaptured = FirefoxScreenshotOcrHandler.Execute(process, Constants.TESS_DATA_PATH, screenshotPath, croppedImagePath);

            //
            return $"Result: {imageCaptured}";
        }
    }
}