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

        public IntPtr WaitForActiveWindow(int timeoutAttempts = 10)
        {
            var endTime = DateTime.Now.AddSeconds(timeoutAttempts);
            while (DateTime.Now < endTime)
            {
                uint processId;
                var activeWindow = WindowsAPI.GetForegroundWindow();
                WindowsAPI.GetWindowThreadProcessId(activeWindow, out processId);
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
