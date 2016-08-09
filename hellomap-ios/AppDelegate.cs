using System;
using System.Linq;
using System.Collections.Generic;

using Foundation;
using UIKit;
using Carto.Ui;

namespace CartoMobileSample
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		static bool WritingViewsProgrammatically = true;

		public const string License = "XTUMwQ0ZRQzRzK205Z1VnTll2ZWZRTDJBK20wSXk2ZjRqQUlVV0VmN2l4Vm95dHFQQ3dLNUY" +
		"2UC8rNVplQ2RvPQoKcHJvZHVjdHM9c2RrLXhhbWFyaW4taW9zLTQuKgpidW5kbGVJZGVudGlmaWVyPWNvbS5u" +
		"dXRpdGVxLmhlbGxvbWFwLnhhbWFyaW4Kd2F0ZXJtYXJrPWNhcnRvZGIKdmFsaWRVbnRpbD0yMDE2LTA5LTAyCm9ubGluZUxpY2Vuc2U9MQo=";

		public override UIWindow Window { get; set; }

		public UINavigationController Controller { get; set; }

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			MapView.RegisterLicense(License);

			if (WritingViewsProgrammatically)
			{
				Controller = new UINavigationController(new MapListController());
				Controller.NavigationBarHidden = false;

				Window = new UIWindow(UIScreen.MainScreen.Bounds);

				Window.RootViewController = Controller;

				Window.MakeKeyAndVisible();
			}
			else {
				Console.WriteLine("Writing views via StoryBoard");
				// Add the following properties and values to Info.plist:
				// [Main storyboard file base name] - [MainStoryboard_iPhone]
				// [Main storyboard file base name (iPad)] - [MainStoryboard_iPad]
			}

			return true;
		}

	}
}

