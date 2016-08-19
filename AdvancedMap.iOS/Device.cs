using System;
using UIKit;

namespace AdvancedMap.iOS
{
	public class Device
	{
		public static bool IsAtLeastiOS8
		{
			get
			{
				return UIDevice.CurrentDevice.CheckSystemVersion(8, 0);
			}
		}

		public static nfloat NavigationBarHeight;

		public static nfloat StatusBarHeight
		{
			get
			{
				return UIApplication.SharedApplication.StatusBarFrame.Height;
			}
		}

		public static nfloat ScreenWidth
		{
			get
			{
				return UIScreen.MainScreen.Bounds.Size.Width;
			}
		}

		public static nfloat ScreenHeight
		{
			get
			{
				return UIScreen.MainScreen.Bounds.Size.Height;
			}
		}

		public static bool IsWidePhone
		{
			get
			{
				return ScreenHeight / ScreenWidth <= 1.5f;
			}
		}

		public static bool IsFullHD
		{
			get
			{
				return ScreenHeight == 736;
			}
		}

		public static nfloat TrueScreenHeight
		{
			get
			{
				return ScreenHeight - (NavigationBarHeight + StatusBarHeight);
			}
		}

		public static bool IsLandscape
		{
			get
			{
				if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft)
				{
					return true;
				}

				if (UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight)
				{
					return true;
				}

				return false;
			}
		}

		public static string AppVersion
		{
			get
			{
				return Foundation.NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
			}
		}

		public static string OsVersion
		{
			get
			{
				return "iOS " + UIDevice.CurrentDevice.SystemVersion;
			}
		}

		public static bool IsIOS8OrHigher
		{
			get
			{
				float result;

				float.TryParse(UIDevice.CurrentDevice.SystemVersion, out result);

				return result >= 8.0f;
			}
		}

		public static bool IsTablet
		{
			get
			{
				return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad;
			}
		}

	}
}

