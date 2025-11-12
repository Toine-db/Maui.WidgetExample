using Android.App;
using Android.Content;

namespace WidgetExample.Platforms.Android.Widgets;

[BroadcastReceiver(Exported = true)]
[IntentFilter([
	MyWidgetProvider.WidgetToAppSilentIntentAction
])]
public class WidgetSilentReceiver : BroadcastReceiver
{
	public override void OnReceive(Context? context, Intent? intent)
	{
		if (context == null
			|| intent == null)
		{
			return;
		}

		if (intent.Action != MyWidgetProvider.WidgetToAppSilentIntentAction)
		{
			return;
		}

		// Extract the counter value from the intent
		var counterValue = intent.GetIntExtra(MyWidgetProvider.WidgetToAppSilentExtraValueField, int.MinValue);
		if (counterValue != int.MinValue)
		{
			Preferences.Set(MainPage.SharedStorageAppIncommingDataKey, counterValue, MainPage.SharedStorageGroupId);
		}

		// Refresh the widgets to reflect any changes
		MainPage.RefreshWidget(); // advisable to move this static method from MainPage and to use 'Context' from this receiver instead	 
	}

}
