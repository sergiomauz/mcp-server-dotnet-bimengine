using System.Diagnostics;


namespace Application.Commons
{
    public class WindowWatcher
    {
        private readonly string _targetProcessName;

        public WindowWatcher(string processName)
        {
            _targetProcessName = processName;
        }

        public IntPtr WaitForActiveWindow(int timeoutSeconds = 10)
        {
            var endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            while (DateTime.Now < endTime)
            {
                uint processId;

                var activeWindow = WindowsAPI.GetForegroundWindow();
                _ = WindowsAPI.GetWindowThreadProcessId(activeWindow, out processId);
                try
                {
                    var proc = Process.GetProcessById((int)processId);
                    if (proc.ProcessName.Equals(_targetProcessName, StringComparison.OrdinalIgnoreCase))
                    {
                        return activeWindow;
                    }
                }
                catch
                {
                    return IntPtr.MinValue;
                }
                Thread.Sleep(500);
            }

            return IntPtr.Zero;
        }
    }
}
