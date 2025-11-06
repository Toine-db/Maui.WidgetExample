namespace WidgetExample
{
	public partial class MainPage : ContentPage
	{
		private const string SharedStorageGroupId = "group.com.enbyin.WidgetExample";
		private const string SharedStorageAppIncommingDataKey = "my.appincomming.data.key";
		private const string SharedStorageAppOutgoingDataKey = "my.appoutgoing.data.key";

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

			// show incomming data if any
			var incommingData = Preferences.Get(SharedStorageAppIncommingDataKey, int.MinValue, SharedStorageGroupId);
			if (incommingData != int.MinValue)
			{
				Message = "value set by incomming data";
				Count = incommingData;
				// clear the incomming data after reading it
				Preferences.Set(SharedStorageAppIncommingDataKey, int.MinValue, SharedStorageGroupId);
			}
			else
			{
				// otherwise use old value set by app
				var outgoingData = Preferences.Get(SharedStorageAppOutgoingDataKey, int.MinValue, SharedStorageGroupId);
				if (outgoingData != int.MinValue)
				{
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
#endif
		}

		private static void RefreshWidget()
		{
#if IOS
			var widgetCenterProxy = new WidgetKit.WidgetCenterProxy();
			//widgetCenterProxy.ReloadAllTimeLines(); // reload all widgets
			widgetCenterProxy.ReloadTimeLinesOfKind("MyWidget"); // reload only my widgets
#endif
		}
	}
}
