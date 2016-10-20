using Carto.Ui;
using Foundation;
using UIKit;

namespace CartoMap.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		const string License = "XTUN3Q0ZFMk5uUEwwQXQyUm5jN3BDeHpDeWdrMGs0VVRBaFFocVQybmRXRkxFcDlhUDBKWWxCN04rRHkwRWc9PQoKYXBwVG9rZW49NDQyNTRiNjQtNTRlNS00Y2Y1LTgzZDMtZDA2ZTU1M2QzOGIzCmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLmNhcnRvbWFwCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1pb3MtNC4qCndhdGVybWFyaz1jdXN0b20K";

		public override UIWindow Window { get; set; }

		public UINavigationController Controller { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			MapView.RegisterLicense(License);

			Controller = new UINavigationController(new MapListController());

			Controller.NavigationBarHidden = false;

			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			Window.RootViewController = Controller;

			Window.MakeKeyAndVisible();

			Device.NavigationBarHeight = Controller.NavigationBar.Frame.Height;

			return true;
		}

	}
}


