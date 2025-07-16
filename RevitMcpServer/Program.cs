#region REVIT-MCP-SERVER
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;


//var builder = Host.CreateApplicationBuilder(args);
//builder.Logging.AddConsole(consoleLogOptions =>
//{
//    // Configure all logs to go to stderr
//    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
//});
//builder.Services
//    .AddMcpServer()
//    .WithStdioServerTransport()
//    .WithToolsFromAssembly();
//await builder.Build().RunAsync();
#endregion



#region CONSOLE-APPLICATION
using Application.UseCases.FirefoxScreenshotOcr;

// Path to the folder with Tesseract data (downloads the .traineddata)
// Downloaded from https://github.com/tesseract-ocr/tessdata
var tessDataPath = @"C:\revit-mcp-server\tessdata";

// Process to detect
var process = "firefox";

// Path to save images
var screenshotPath = $"C:\\revit-mcp-server\\captures\\firefox_{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
var croppedImagePath = $"C:\\revit-mcp-server\\captures\\firefox_cropped_{DateTime.Now:yyyyMMdd_HHmmss}.png";

// 
var textDetected = FirefoxScreenshotOcrHandler.Execute(process, tessDataPath, screenshotPath, croppedImagePath);
Console.WriteLine(textDetected);
#endregion
