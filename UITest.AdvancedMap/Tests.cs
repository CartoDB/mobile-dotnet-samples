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
			app.ScrollDown();
			app.Screenshot("First screen.");
		}

	}
}

