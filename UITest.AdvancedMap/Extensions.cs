using System;
using System.Linq;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest.AdvancedMap
{
	public static class Extensions
	{
		public static IApp Start(this Platform platform)
		{
			if (platform == Platform.Android)
			{
				return ConfigureApp.Android.EnableLocalScreenshots().StartApp();
			}

			return ConfigureApp.iOS.EnableLocalScreenshots().StartApp();
		}

		public static void Click(this IApp app, AppResult item)
		{
			app.TapCoordinates(item.Rect.CenterX, item.Rect.CenterY);
		}

	}
}

