using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace WidgetExample.Platforms.Android.Widgets;

[BroadcastReceiver(Label = "My Widget", Exported = true)]
[IntentFilter(new[]
{
	// listens to these specific intents
	AppWidgetManager.ActionAppwidgetUpdate,
	IncrementCounterIntentAction,
	DecrementCounterIntentAction
})]
[MetaData(AppWidgetManager.MetaDataAppwidgetProvider, Resource = "@xml/mywidget_provider_info")]
public class MyWidgetProvider : AppWidgetProvider
{
	public const string IncrementCounterIntentAction = "com.enbyin.WidgetExample.INCREMENT_COUNTER";
	public const string DecrementCounterIntentAction = "com.enbyin.WidgetExample.DECREMENT_COUNTER";
	public const string WidgetToAppSilentIntentAction = "com.enbyin.WidgetExample.WIDGET_TO_APP_SILENT_TRIGGER";
	public const string WidgetToAppSilentExtraValueField = "my_counter";

	public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
	{
		if (context == null
			|| appWidgetManager == null
			|| appWidgetIds == null)
		{
			return;
		}

		// only update when widgets specific ID is mentioned
		foreach (var appWidgetId in appWidgetIds)
		{
			var views = BuildRemoteViews(context, appWidgetId);
			appWidgetManager.UpdateAppWidget(appWidgetId, views);
		}
	}

	public override void OnReceive(Context? context, Intent? intent)
	{
		if (intent == null
			|| context == null)
		{
			base.OnReceive(context, intent);
			return;
		}

		switch (intent.Action)
		{
			case IncrementCounterIntentAction:
				{
					var currentCount = SharedStorageHelper.GetBestStoredDataCount();
					currentCount++;

					// Send silent trigger to app, that will in background do the logic and refresh this widget remotely
					var silentIntent = new Intent(context, typeof(WidgetSilentReceiver));
					silentIntent.SetAction(WidgetToAppSilentIntentAction);
					silentIntent.PutExtra(WidgetToAppSilentExtraValueField, currentCount);
					silentIntent.SetPackage(context.PackageName);

					context.SendBroadcast(silentIntent);

					return;
				}
			case DecrementCounterIntentAction:
				{
					var currentCount = SharedStorageHelper.GetBestStoredDataCount();
					currentCount--;

					// Do logic and refreshing view here in widget
					Preferences.Set(MainPage.SharedStorageAppIncommingDataKey, currentCount, MainPage.SharedStorageGroupId);
					UpdateAllWidgets(context);

					return;
				}
		}

		// this will trigger a OnUpdate() in this AppWidgetProvider during 'AppWidgetManager.ActionAppwidgetUpdate'
		base.OnReceive(context, intent);
	}

	public static void UpdateAllWidgets(Context context)
	{
		var appWidgetManager = AppWidgetManager.GetInstance(context);
		var thisWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(MyWidgetProvider)).Name);
		var appWidgetIds = appWidgetManager?.GetAppWidgetIds(thisWidget);
		if (appWidgetIds == null)
		{
			return;
		}

		foreach (var appWidgetId in appWidgetIds)
		{
			var views = BuildRemoteViews(context, appWidgetId);
			appWidgetManager?.UpdateAppWidget(appWidgetId, views);
		}
	}

	public override void OnDeleted(Context? context, int[]? appWidgetIds)
	{
		base.OnDeleted(context, appWidgetIds);

		if (context == null || appWidgetIds == null)
		{
			return;
		}

		foreach (var widgetId in appWidgetIds)
		{
			WidgetConfigurationActivity.DeleteWidgetConfiguration(widgetId);
		}
	}

	private static RemoteViews BuildRemoteViews(Context context, int widgetId)
	{
		var views = new RemoteViews(context.PackageName, Resource.Layout.mywidget);

		var message = string.Empty;

		// not the best mechanism but for works fine for this demo
		// - incomming data is set by Widget, has prio 1
		// - outgoing data is set by app, has prio 2
		// - incomming data is reset as soon as app starts, making outgoing data visible
		var currentCount = Preferences.Get(MainPage.SharedStorageAppIncommingDataKey, int.MinValue, MainPage.SharedStorageGroupId);
		if (currentCount == int.MinValue) // check if default value is found, no data
		{
			currentCount = Preferences.Get(MainPage.SharedStorageAppOutgoingDataKey, int.MinValue, MainPage.SharedStorageGroupId);
			if (currentCount == int.MinValue)  // check if default value is found, no data
			{
				currentCount = 0;
			}
			else
			{
				message = "value received from app";
			}
		}

		views.SetTextViewText(Resource.Id.widgetText, $"{currentCount}");
		views.SetTextViewText(Resource.Id.widgetMessage, message);

		var favoriteConfiguredEmoji = WidgetConfigurationActivity.GetConfiguredEmoji(widgetId);
		views.SetTextViewText(Resource.Id.widgetEmojiLeft, favoriteConfiguredEmoji);
		views.SetTextViewText(Resource.Id.widgetEmojiRight, favoriteConfiguredEmoji);

		// unique request code for 'pending' intents:
		// they must be unique within package + action + flags, otherwise the pending intent will be overwritten/shared
		// this can be important when an intent action must be fonr by a specific WidgetId, but this is not always required
		var incrementRequestCode = widgetId * 100 + 1;
		var decrementRequestCode = widgetId * 100 + 2;
		var openAppRequestCode = widgetId * 100 + 3;

		// Attach intent to increment button
		var incrementIntent = new Intent(context, typeof(MyWidgetProvider));
		incrementIntent.SetAction(IncrementCounterIntentAction);
		var incrementPendingIntent = PendingIntent.GetBroadcast(
			context,
			incrementRequestCode,
			incrementIntent,
			PendingIntentFlags.UpdateCurrent | (Build.VERSION.SdkInt >= BuildVersionCodes.S ? PendingIntentFlags.Mutable : 0)
		);
		views.SetOnClickPendingIntent(Resource.Id.widgetIncrementButton, incrementPendingIntent);

		// Attach intent to decrement button
		var decrementIntent = new Intent(context, typeof(MyWidgetProvider));
		decrementIntent.SetAction(DecrementCounterIntentAction);
		var decrementPendingIntent = PendingIntent.GetBroadcast(
			context,
			decrementRequestCode,
			decrementIntent,
			PendingIntentFlags.UpdateCurrent | (Build.VERSION.SdkInt >= BuildVersionCodes.S ? PendingIntentFlags.Mutable : 0)
		);
		views.SetOnClickPendingIntent(Resource.Id.widgetDecrementButton, decrementPendingIntent);

		// Attach intent to open the app when widget background/text is tapped
		var openAppIntent = new Intent(Intent.ActionView);
		openAppIntent.SetData(global::Android.Net.Uri.Parse($"{App.UrlScheme}://{App.UrlHost}?counter={currentCount}"));
		openAppIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
		var openAppPendingIntent = PendingIntent.GetActivity(
			context,
			openAppRequestCode,
			openAppIntent,
			PendingIntentFlags.UpdateCurrent | (Build.VERSION.SdkInt >= BuildVersionCodes.S ? PendingIntentFlags.Immutable : 0)
		);
		views.SetOnClickPendingIntent(Resource.Id.widgetText, openAppPendingIntent);

		return views;
	}
}