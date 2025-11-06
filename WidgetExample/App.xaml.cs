namespace WidgetExample
{
	public partial class App : Application
	{
		public const string UrlScheme = "widgetexample";
		public const string UrlHost = "widget";

		public App()
		{
			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			var window = new Window(new AppShell());

			window.Resumed += (s, e) =>
			{
				// Notify MainPage to reload data when app resumes
				if (window.Page is AppShell { CurrentPage: MainPage mainPage })
				{
					mainPage.OnResumed();
				}
			};

			return window;
		}

		internal static void HandleWidgetUrl(Uri uri)
		{
			if (uri is { Scheme: UrlScheme, Host: UrlHost })
			{
				var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
				var counterValue = query["counter"];

				if (!string.IsNullOrEmpty(counterValue)
					&& int.TryParse(counterValue, out var urlCounterValue))
				{
					NotifyMainPageOfIncomingData(urlCounterValue);
				}
			}
		}

		private static void NotifyMainPageOfIncomingData(int urlCounterValue)
		{
			var app = Microsoft.Maui.Controls.Application.Current;

			app?.Dispatcher.Dispatch(() =>
			{
				if (app?.Windows?.Count > 0 &&
					app.Windows[0].Page is AppShell { CurrentPage: MainPage mainPage })
				{
					mainPage.OnResumedByUrl(urlCounterValue);
				}
			});
		}
	}
}