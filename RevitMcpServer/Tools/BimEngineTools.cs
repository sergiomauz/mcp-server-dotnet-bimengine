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
            var process = "firefox";
            var screenshotPath = $"C:\\revit-mcp-server\\captures\\firefox_screenshot_{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var croppedImagePath = $"C:\\revit-mcp-server\\captures\\firefox_cropped_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            var imageCaptured = FirefoxScreenshotOcrHandler.Execute(process, screenshotPath, croppedImagePath);

            return $"Result: {imageCaptured}";
        }
    }
}