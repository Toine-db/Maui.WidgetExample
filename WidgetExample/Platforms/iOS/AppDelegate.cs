using Foundation;
using UIKit;

namespace WidgetExample
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        private bool _isBackgroundLaunch = false;

        protected override MauiApp CreateMauiApp()
        {
            // If woken by silent push, use minimal initialization
            return _isBackgroundLaunch
                ? MauiProgram.CreateMinimalMauiApp()
                : MauiProgram.CreateMauiApp();
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // IMPORTANT: This has NOT been fully tested with background push notifications.....yet  

            // Detect if launched by remote notification in background
            if (launchOptions != null &&
                launchOptions.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
            {
                _isBackgroundLaunch = true;
            }

            return base.FinishedLaunching(application, launchOptions);
        }

        public override bool OpenUrl(UIKit.UIApplication application, NSUrl url, NSDictionary options)
        {
            if (url.Scheme == App.UrlScheme)
            {
                var uri = new Uri(url.AbsoluteString);
                App.HandleWidgetUrl(uri);

                return true;
            }

            return base.OpenUrl(application, url, options);
        }
    }
}
