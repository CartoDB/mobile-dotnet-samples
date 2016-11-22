
using Carto.Ui;
using Foundation;
using HockeyApp.iOS;
using Shared.iOS;
using UIKit;

namespace CartoMap.iOS
{
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		const string License = "XTUN3Q0ZFMk5uUEwwQXQyUm5jN3BDeHpDeWdrMGs0VVRBaFFocVQybmRXRkxFcDlhUDBKWWxCN04rRHkwRWc9PQoKYXBwVG9rZW49NDQyNTRiNjQtNTRlNS00Y2Y1LTgzZDMtZDA2ZTU1M2QzOGIzCmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLmNhcnRvbWFwCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1pb3MtNC4qCndhdGVybWFyaz1jdXN0b20K";
		const string HockeyId = "0d9f582ec2c348598d1712d640ee16c4";

		public override UIWindow Window { get; set; }

		public UINavigationController Controller { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			BITHockeyManager manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure(HockeyId);
			manager.DisableUpdateManager = false;
			manager.StartManager();

			MapView.RegisterLicense(License);

			Controller = new UINavigationController(new MapListController("CARTO Mobile Samples", Samples.RowSources));

			Controller.NavigationBarHidden = false;

			Window = new UIWindow(UIScreen.MainScreen.Bounds);

			Window.RootViewController = Controller;

			Window.MakeKeyAndVisible();

			Device.NavigationBarHeight = Controller.NavigationBar.Frame.Height;

			return true;
		}

	}
}


