using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace HelloMap.Forms.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public static CoreGraphics.CGRect ScreenBounds { get; set; }

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			ScreenBounds = UIScreen.MainScreen.Bounds;

			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}
