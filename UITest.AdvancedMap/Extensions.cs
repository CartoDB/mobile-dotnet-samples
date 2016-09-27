using System;
using Xamarin.UITest;

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
	}
}

