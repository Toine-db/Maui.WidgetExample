namespace WidgetExample.Platforms.Android.Resources.Widgets;

internal static class SharedStorageHelper
{
	public static Int32 GetBestStoredDataCount()
	{
		var currentCount = Preferences.Get(MainPage.SharedStorageAppIncommingDataKey, int.MinValue, MainPage.SharedStorageGroupId);
		if (currentCount == int.MinValue) // check if default value is found, no data
		{
			currentCount = Preferences.Get(MainPage.SharedStorageAppOutgoingDataKey, int.MinValue, MainPage.SharedStorageGroupId);
			if (currentCount == int.MinValue)  // check if default value is found, no data
			{
				currentCount = 0;
			}
		}

		return currentCount;
	}
}
