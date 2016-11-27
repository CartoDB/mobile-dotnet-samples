using System;
using Android.App;
using Android.Runtime;
using Carto.Ui;
using Carto.Utils;
using HockeyApp.Android;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Application]
	public class MapApplication : Application
	{
		const string CartoLicense = "XTUN3Q0ZCQmk4WTVWSWJ1OXpPMkU0RndCK3pQV3ZDczVBaFFCQUw4T2pnWllXb1UzRWJoUkVHOVEzOE9pTmc9PQoKYXBwVG9rZW49NWNkNTVkZGQtMGMwOS00ZmQyLThiYjQtNjM5YzQ4OGMyNDQ5CnBhY2thZ2VOYW1lPWNvbS5jYXJ0by54YW1hcmluLmNhcnRvCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1hbmRyb2lkLTQuKgp3YXRlcm1hcms9Y3VzdG9tCg==";
		public const string HockeyId = "2e7217323aaf4ca48f66a1518497b744";

		public MapApplication(IntPtr a, JniHandleOwnership b) : base (a, b)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();

			MapBaseActivity.ViewResource = Resource.Layout.Main;
			MapBaseActivity.MapViewResource = Resource.Id.mapView;

			Log.ShowError = true;
			Log.ShowWarn = true;

			// Register license
			MapView.RegisterLicense(CartoLicense, ApplicationContext);

			CrashManager.Register(this, HockeyId);
		}
	}
}

