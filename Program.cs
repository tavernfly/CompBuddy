using CompBuddy.CompBuddy;
using System.Runtime.InteropServices;

namespace CompBuddy
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDpiAwarenessContext(int dpiAwarenessContext);

        const int DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = -4;

        [STAThread]
        static void Main()
        {
            SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE);

            // Initialize application configuration
            ApplicationConfiguration.Initialize();

            // Start the Tracker form
            Application.Run(new Tracker());
        }
    }
}