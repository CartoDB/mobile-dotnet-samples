using Carto.Ui;
using Foundation;
using UIKit;

namespace HelloMap.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		const string License = "XTUMwQ0ZRQzh2bG5QUTgxUkd1RUIrT2xNT2J4R0thNU1FUUlVZE43dllEMGI5Y0R1QldYSllXdmN4M21mNUljPQoKYXBwVG9rZW49ODVkZDM5ZTMtYjkyZS00MWM0LThkMWYtY2VlMWQ0ZWNiZmVjCmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLmhlbGxvbWFwCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1pb3MtNC4qCndhdGVybWFyaz1jYXJ0b2RiCg==";
		public override UIWindow Window { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			MapView.RegisterLicense(License);

			return true;
		}
	}
}


