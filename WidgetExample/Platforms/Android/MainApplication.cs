using Android.App;
using Android.OS;
using Android.Runtime;

namespace WidgetExample
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp()
        {
            bool isBackgroundOnly = IsBackgroundExecution();

            return isBackgroundOnly
                ? MauiProgram.CreateMinimalMauiApp()
                : MauiProgram.CreateMauiApp();
        }

        /// <summary>
        /// Detects if app is running in background (widget updates, silent receivers, services)
        /// </summary>
        /// <returns>true if background execution, otherwise false.</returns>
        private bool IsBackgroundExecution()
        {
            try
            {
                var activityManager = (ActivityManager?)GetSystemService(ActivityService);
                if (activityManager == null)
                {
                    return false; // Assume foreground if service not available
                }

                var runningAppProcesses = activityManager.RunningAppProcesses;
                if (runningAppProcesses == null)
                {
                    return false; // Assume foreground if service not available
                }

                foreach (var processInfo in runningAppProcesses)
                {
                    if (processInfo.Pid == Process.MyPid())
                    {
                        // Check process importance
                        // Foreground (100) = Activity in foreground → full MAUI
                        // ForegroundService (125) = Widget/service only → minimal MAUI
                        // Visible (200) = Activity visible but not focused → full MAUI
                        // Service (300+) = Background service → minimal MAUI
                        // safe usage: Anything above Visible is considered background-only
                        bool isBackground = (int)processInfo.Importance > (int)Importance.Visible;

                        return isBackground;
                    }
                }
            }
            catch
            {
                // ignore errors and assume foreground
            }

            // On error, or process not found, assume foreground to avoid breaking UI launch
            return false;
        }
    }
}
