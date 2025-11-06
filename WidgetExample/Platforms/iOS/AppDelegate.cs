using Foundation;

namespace WidgetExample
{
	[Register("AppDelegate")]
	public class AppDelegate : MauiUIApplicationDelegate
	{
		protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

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
