using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Widget;

namespace WidgetExample.Platforms.Android.Resources.Widgets;

[BroadcastReceiver(Label = "My Widget", Exported = true)]
[IntentFilter(new[] { AppWidgetManager.ActionAppwidgetUpdate })]
[MetaData(AppWidgetManager.MetaDataAppwidgetProvider, Resource = "@xml/mywidget_provider_info")]
public class MyWidgetProvider : AppWidgetProvider
{
	public override void OnUpdate(Context? context, AppWidgetManager? appWidgetManager, int[]? appWidgetIds)
	{
		if (context == null
			|| appWidgetIds == null
			|| appWidgetManager == null)
		{
			return;
		}

		// Update all widgets for specified IDs
		foreach (var appWidgetId in appWidgetIds)
		{
			var views = BuildRemoteViews(context);
			appWidgetManager.UpdateAppWidget(appWidgetId, views);
		}
	}

	private static RemoteViews BuildRemoteViews(Context context)
	{
		var views = new RemoteViews(context.PackageName, Resource.Layout.mywidget_simple);
		views.SetTextViewText(Resource.Id.widgetText, "Count: 5 (static)");

		return views;
	}
}