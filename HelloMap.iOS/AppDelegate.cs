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
        const string License = "XTUN3Q0ZCbStWd1B2RWpCekJndE5EelFQSmtKWWg5UGpBaFFDNmgyTGt3S1EwUHgwWXlVbk8weEY4RXF5VHc9PQoKYXBwVG9rZW49MTA2MjhjYTUtNjkzYi00NTFhLTgyY2QtNjE2OWNkZmU0ZWQwCmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLmhlbGxvLnhhbQpvbmxpbmVMaWNlbnNlPTEKcHJvZHVjdHM9c2RrLXhhbWFyaW4taW9zLTQuKgp3YXRlcm1hcms9Y3VzdG9tCg==";
		public override UIWindow Window { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			MapView.RegisterLicense(License);

			return true;
		}
	}
}


