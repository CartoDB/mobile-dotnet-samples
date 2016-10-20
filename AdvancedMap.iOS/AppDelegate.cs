
using Carto.Ui;
using Foundation;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		const string License = "XTUMwQ0ZRREVzQjVXdUhMb3V3eGVIeU9XN2JvUHNtSm9kd0lVQkRrVkp2T282SlRpSFVlNUIvMDFJNmdmTVg0" +
			"PQoKcHJvZHVjdHM9c2RrLXhhbWFyaW4taW9zLTQuKgpidW5kbGVJZGVudGlmaWVyPWNvbS5jYXJ0by5hZHZhbmNlZG1hcC54YW1hcmlu" +
			"Lmlvcwp3YXRlcm1hcms9ZGV2ZWxvcG1lbnQKdmFsaWRVbnRpbD0yMDE2LTA5LTE4Cm9ubGluZUxpY2Vuc2U9MQo=";

		public override UIWindow Window { get; set; }

		public UINavigationController Controller { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			MapView.RegisterLicense(License);

			UIViewController initial = new MapListController("Advanced Map Samples", Samples.RowSources);
			Controller = new UINavigationController(initial);

			Controller.NavigationBarHidden = false;

			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			Window.RootViewController = Controller;

			Window.MakeKeyAndVisible();

			Device.NavigationBarHeight = Controller.NavigationBar.Frame.Height;

			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			return true;
		}

	}
}


