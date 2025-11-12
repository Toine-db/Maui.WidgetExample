namespace WidgetExample
{
	public partial class MainPage : ContentPage
	{

		public const string SharedStorageGroupId = "group.com.enbyin.WidgetExample";
		public const string SharedStorageAppIncommingDataKey = "my.appincomming.data.key";
		public const string SharedStorageAppOutgoingDataKey = "my.appoutgoing.data.key";

		public MainPage()
		{
			InitializeComponent();
			BindingContext = this;
		}

		private int _count = 0;
		public int Count
		{
			get => _count;
			set
			{
				if (_count == value)
				{
					return;
				}

				_count = value;
				OnPropertyChanged();
			}
		}

		private string _message = string.Empty;
		public string Message
		{
			get => _message;
			set
			{
				_message = value;
				OnPropertyChanged();
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			LoadIncomingData();
		}

		public void OnResumed()
		{
			LoadIncomingData();
		}

		public void OnResumedByUrl(int incommingCount)
		{
			// Note: OpenUrl is called after OnResumed

			Count = incommingCount;
			Message = "value set by opening URL";

			// Sync outgoing data to same value
			Preferences.Set(SharedStorageAppOutgoingDataKey, Count, SharedStorageGroupId);
		}

		private void LoadIncomingData()
		{
			// show incomming data if any
			var incommingData = Preferences.Get(SharedStorageAppIncommingDataKey, int.MinValue, SharedStorageGroupId);
			if (incommingData != int.MinValue)
			{
				Message = "value set by incomming data";
				Count = incommingData;

				// Sync outgoing data to same value
				Preferences.Set(SharedStorageAppOutgoingDataKey, Count, SharedStorageGroupId);

				// clear the incomming data after reading it
				Preferences.Set(SharedStorageAppIncommingDataKey, int.MinValue, SharedStorageGroupId);
			}
			else
			{
				// otherwise use old value set by app
				var outgoingData = Preferences.Get(SharedStorageAppOutgoingDataKey, int.MinValue, SharedStorageGroupId);
				if (outgoingData != int.MinValue)
				{
					Message = string.Empty;
					Count = outgoingData;
				}
			}
		}

		private void OnCounterAddClicked(object? sender, EventArgs e)
		{
			Count++;
			Message = string.Empty;

			ShareData();
			RefreshWidget();
		}

		private void OnCounterSubtractClicked(object? sender, EventArgs e)
		{
			Count--;
			Message = string.Empty;

			ShareData();
			RefreshWidget();
		}

		private void ShareData()
		{
			var outgoingData = Count;
			Preferences.Set(SharedStorageAppOutgoingDataKey, outgoingData, SharedStorageGroupId);

#if IOS
			// Force synchronization of the shared user defaults
			// (probably not necessary, just in case)
			var userDefaults = new Foundation.NSUserDefaults(SharedStorageGroupId, Foundation.NSUserDefaultsType.SuiteName);
			userDefaults.Synchronize();
#elif ANDROID
			// No special action needed for Android SharedPreferences
#endif
		}

		public static void RefreshWidget()
		{
#if IOS
			var widgetCenterProxy = new WidgetKit.WidgetCenterProxy();
			//widgetCenterProxy.ReloadAllTimeLines(); // reload all widgets
			widgetCenterProxy.ReloadTimeLinesOfKind("MyWidget"); // reload only my widgets
#elif ANDROID

			var context = Android.App.Application.Context;

			// Use AppWidgetManager to get all widget IDs for this provider
			var appWidgetManager = Android.Appwidget.AppWidgetManager.GetInstance(context);
			var componentName = new Android.Content.ComponentName(context, Java.Lang.Class.FromType(typeof(Platforms.Android.Widgets.MyWidgetProvider)));
			var appWidgetIds = appWidgetManager?.GetAppWidgetIds(componentName);

			// Create an intent to update the widget
			var intent = new Android.Content.Intent(Android.Appwidget.AppWidgetManager.ActionAppwidgetUpdate);
			intent.SetPackage(context.PackageName); // only widgets from this app
			intent.PutExtra(Android.Appwidget.AppWidgetManager.ExtraAppwidgetIds, appWidgetIds); // only widgets with these IDs

			// Send the broadcast to update the widget
			context.SendBroadcast(intent);
#endif
		}
	}
}
