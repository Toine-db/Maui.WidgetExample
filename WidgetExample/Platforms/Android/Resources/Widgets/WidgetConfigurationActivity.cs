using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;

using Button = global::Android.Widget.Button;

namespace WidgetExample.Platforms.Android.Resources.Widgets;

[Activity(Label = "Configure Widget",
	Exported = true,
	Name = "widgetexample.WidgetConfigurationActivity",
	Theme = "@android:style/Theme.Material.Light.Dialog",
	ConfigurationChanges = ConfigChanges.UiMode)]
[IntentFilter([AppWidgetManager.ActionAppwidgetConfigure])]
public class WidgetConfigurationActivity : Activity
{
	private int _appWidgetId = AppWidgetManager.InvalidAppwidgetId;
	private EditText? _emojiEditText;

	private const string DefaultEmoji = "😃";

	private static string GetConfigurationStorageKey(int widgetId) => $"emoji_{widgetId}";

	protected override void OnCreate(Bundle? savedInstanceState)
	{
		base.OnCreate(savedInstanceState);

		// Set result to CANCELED in case user backs out
		SetResult(Result.Canceled);

		// Get the widget ID from the intent
		var extras = Intent?.Extras;
		if (extras != null)
		{
			_appWidgetId = extras.GetInt(AppWidgetManager.ExtraAppwidgetId, AppWidgetManager.InvalidAppwidgetId);
		}

		// If the intent didn't have a widget ID, finish
		if (_appWidgetId == AppWidgetManager.InvalidAppwidgetId)
		{
			Finish();

			return;
		}

		// Ui is normally created with a layout xml file, and loaded with SetContentView(Resource.Layout.X)
		// For this demo the UI is created in code..... in my opinion this quickly becomes messy because logic becomes mixed with presentation
		var layout = new LinearLayout(this)
		{
			Orientation = Orientation.Vertical,
			LayoutParameters = new ViewGroup.LayoutParams(
				ViewGroup.LayoutParams.MatchParent,
				ViewGroup.LayoutParams.WrapContent)
		};
		layout.SetPadding(32, 32, 32, 32);

		var title = new TextView(this)
		{
			Text = "Configure Your Widget",
			TextSize = 20
		};
		title.SetPadding(0, 0, 0, 24);
		layout.AddView(title);

		var label = new TextView(this)
		{
			Text = "Enter your favorite emoji:"
		};
		label.SetPadding(0, 0, 0, 8);
		layout.AddView(label);

		_emojiEditText = new EditText(this)
		{
			Hint = DefaultEmoji,
			Text = GetConfiguredEmoji(_appWidgetId)
		};
		layout.AddView(_emojiEditText);

		var presetLayout = new LinearLayout(this)
		{
			Orientation = Orientation.Horizontal,
			LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
		};
		presetLayout.SetPadding(0, 16, 0, 16);

		void addEmojiButton(ViewGroup parent, string emoji)
		{
			var button = new Button(this)
			{
				Text = emoji,
				LayoutParameters = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1.0f)
			};
			button.Click += (s, e) =>
			{
				if (_emojiEditText != null)
				{
					_emojiEditText.Text = emoji;
				}
			};

			parent.AddView(button);
		}

		addEmojiButton(presetLayout, "😀");
		addEmojiButton(presetLayout, "🤩");
		addEmojiButton(presetLayout, "😎");
		addEmojiButton(presetLayout, "🥳");
		addEmojiButton(presetLayout, "😍");

		layout.AddView(presetLayout);

		// Save button
		var saveButton = new Button(this)
		{
			Text = "Save"
		};
		saveButton.Click += (s, e) =>
		{
			if (_emojiEditText == null)
			{
				return;
			}

			// Get the emoji from the EditText or use a default
			var emoji = _emojiEditText.Text ?? DefaultEmoji;

			// Save the emoji preference
			var key = GetConfigurationStorageKey(_appWidgetId);
			Preferences.Default.Set(key, emoji);

			// Update the widget
			MyWidgetProvider.UpdateAllWidgets(this);

			// Return success
			var resultValue = new Intent();
			resultValue.PutExtra(AppWidgetManager.ExtraAppwidgetId, _appWidgetId);
			SetResult(Result.Ok, resultValue);
			Finish();
		};

		layout.AddView(saveButton);

		SetContentView(layout);
	}

	public static string GetConfiguredEmoji(int widgetId)
	{
		var key = GetConfigurationStorageKey(widgetId);
		var emoji = Preferences.Default.Get<string>(key, DefaultEmoji);

		return emoji;
	}

	public static void DeleteWidgetConfiguration(int widgetId)
	{
		var key = GetConfigurationStorageKey(widgetId);
		Preferences.Default.Remove(key);
	}
}