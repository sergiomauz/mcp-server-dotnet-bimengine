using System.ComponentModel;
using ModelContextProtocol.Server;
using Application.UseCases.Screenshot;


namespace RevitMcpServer.Tools
{
    [McpServerToolType]
    public static class RevitTools
    {
        [McpServerTool, Description("Echoes the message back to the client.")]
        public static string ScreenshotRevit(string message)
        {

            return $"Hello Revit! {message}";
        }

        [McpServerTool, Description("Captures a screenshot of a Firefox window. Wait for 10 attempts, then it fails.")]
        public static object ScreenshotFirefox(string message)
        {
            var process = "firefox";
            var screenshotPath = $"C:\\revit-mcp-server\\captures\\firefox_{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            var croppedImagePath = $"C:\\revit-mcp-server\\captures\\firefox_cropped_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            var imageCaptured = ScreenshotOcrHandler.Execute(process, screenshotPath, croppedImagePath);

            return $"Domain: {imageCaptured}";
        }
    }
}