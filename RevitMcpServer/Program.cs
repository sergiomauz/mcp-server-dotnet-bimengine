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


using Application.UseCases.Screenshot;


var process = "firefox";
var screenshotPath = $"C:\\revit-mcp-server\\captures\\firefox_{process}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
var croppedImagePath = $"C:\\revit-mcp-server\\captures\\firefox_cropped_{DateTime.Now:yyyyMMdd_HHmmss}.png";
var imageCaptured = ScreenshotOcrHandler.Execute(process, screenshotPath, croppedImagePath);

Console.WriteLine(imageCaptured);