using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace WidgetExample
{
	[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
	[IntentFilter(new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataScheme = App.UrlScheme,
		DataHost = App.UrlHost)]
	public class MainActivity : MauiAppCompatActivity
	{
		protected override void OnCreate(Bundle? savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			HandleIntent(Intent);
		}

		protected override void OnNewIntent(Intent? intent)
		{
			base.OnNewIntent(intent);
			HandleIntent(intent);
		}

		private static void HandleIntent(Intent? intent)
		{
			if (intent?.Data != null)
			{
				var uri = new Uri(uriString: intent.Data.ToString());
				App.HandleWidgetUrl(uri);
			}
		}
	}
}
