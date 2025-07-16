using System.ComponentModel;
using ModelContextProtocol.Server;
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

        [McpServerTool, Description("Captures a screenshot of a Firefox window. Wait for 10 attempts, then it fails.")]
        public static object FirefoxScreenshotOcr(string message)
        {
            // Path to the folder with Tesseract data (downloads the .traineddata)
            // Downloaded from https://github.com/tesseract-ocr/tessdata
            var tessDataPath = @"C:\revit-mcp-server\tessdata";

            // Process to detect
            var process = "firefox";

            // Path to save images
            var screenshotPath = $"C:\\revit-mcp-server\\captures\\firefox_{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var croppedImagePath = $"C:\\revit-mcp-server\\captures\\firefox_cropped_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            //
            var imageCaptured = FirefoxScreenshotOcrHandler.Execute(process, tessDataPath, screenshotPath, croppedImagePath);

            //
            return $"Result: {imageCaptured}";
        }
    }
}