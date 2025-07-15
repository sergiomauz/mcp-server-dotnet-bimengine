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
            var savePath = $"C:\\revit-mcp-server\\captures\\firefox_{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            var imageCaptured = ScreenshotOcrHandler.Execute(process, savePath);
            if (!string.IsNullOrEmpty(imageCaptured))
            {
                // Convert image to base64 for any porpouse, could be OCR
                var imageBytes = File.ReadAllBytes(savePath);
                var base64Image = Convert.ToBase64String(imageBytes);

            }

            return $"Domain: {imageCaptured}";
        }
    }
}