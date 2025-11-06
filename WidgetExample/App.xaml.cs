namespace WidgetExample
{
	public partial class App : Application
	{
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
				if (window.Page is AppShell shell &&
					shell.CurrentPage is MainPage mainPage)
				{
					mainPage.OnResumed();
				}
			};

			return window;
		}
	}
}