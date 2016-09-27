using System;
using System.Configuration;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace UITest.AdvancedMap
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		public Tests(Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = platform.Start();
		}

		[Test]
		public void AppLaunches()
		{
			if (platform == Platform.Android)
			{
				// Screenshots produce UnauthorizedException on iOS for some reason
				app.Screenshot("First screen.");
			}

			AppResult[] items = app.GetListItems(platform);

			foreach (AppResult item in items)
			{
				app.Click(item);

				app.Back();
			}
		}

	}
}

