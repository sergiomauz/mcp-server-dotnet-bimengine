#region REVIT-MCP-SERVER
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
await builder.Build().RunAsync();
#endregion



#region CONSOLE-APPLICATION
//using Commons;
//using Application.UseCases.FirefoxScreenshotOcr;

//// Process to detect
//var process = "firefox";

//// Path to save images
//var screenshotPath = $"{Constants.CAPTURES}\\firefox_{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
//var croppedImagePath = $"{Constants.CAPTURES}\\firefox_cropped_{DateTime.Now:yyyyMMdd_HHmmss}.png";

//// Screenshot and OCR
//Console.WriteLine("Ready, you can put Firefox in the foreground to continue");
//var textDetected = FirefoxScreenshotOcrHandler.Execute(process, Constants.TESS_DATA_PATH, screenshotPath, croppedImagePath);
//Console.WriteLine(textDetected);
#endregion
