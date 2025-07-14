using Application.Commons;


namespace Application.UseCases.Screenshot
{
    public static class ScreenshotHandler
    {
        public static bool Execute(string processName, string savePath)
        {
            var watcher = new WindowWatcher(processName);
            var windowHandle = watcher.WaitForActiveWindow();
            if (windowHandle != nint.Zero)
            {
                var capturer = new ScreenCapturer();
                return capturer.CaptureWindow(windowHandle, savePath);
            }

            return false;
        }
    }
}
