using Foundation;

namespace WidgetExample
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool OpenUrl(UIKit.UIApplication application, NSUrl url, NSDictionary options)
        {
            if (url.Scheme == "widgetexample")
            {
                HandleWidgetUrl(url);
                return true;
            }
            return base.OpenUrl(application, url, options);
        }

        private void HandleWidgetUrl(NSUrl url)
        {
            if (url.Host == "openapp")
            {
                var query = url.Query;
                if (!string.IsNullOrEmpty(query))
                {
                    var parameters = ParseQueryString(query);
                    if (parameters.TryGetValue("count", out string? countValue) && 
                        !string.IsNullOrEmpty(countValue) &&
                        int.TryParse(countValue, out int count))
                    {
                        // Store the count from the widget intent
                        const string SharedStorageGroupId = "group.com.enbyin.WidgetExample";
                        const string SharedStorageAppIncommingDataKey = "my.appincomming.data.key";
                        
                        Microsoft.Maui.Storage.Preferences.Set(SharedStorageAppIncommingDataKey, count, SharedStorageGroupId);
                        
                        // Notify the main page if it's already loaded
                        NotifyMainPageOfIncomingData();
                    }
                }
            }
        }

        private Dictionary<string, string> ParseQueryString(string query)
        {
            var result = new Dictionary<string, string>();
            var pairs = query.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    result[keyValue[0]] = Uri.UnescapeDataString(keyValue[1]);
                }
            }
            return result;
        }

        private void NotifyMainPageOfIncomingData()
        {
            Microsoft.Maui.Controls.Application.Current?.Dispatcher.Dispatch(() =>
            {
                var app = Microsoft.Maui.Controls.Application.Current;
                if (app?.Windows?.Count > 0 && 
                    app.Windows[0].Page is AppShell shell &&
                    shell.CurrentPage is MainPage mainPage)
                {
                    mainPage.OnResumed();
                }
            });
        }
    }
}
