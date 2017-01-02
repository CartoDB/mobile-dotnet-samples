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
		const string CartoLicense = "XTUN3Q0ZFOGxVaUg0ZzhsRzlURlE3NTFtTmhMb3RocFdBaFJNcyt4UEw5aVdXcXdtMWdlWXdIdWh0eVhxZ0E9PQoKYXBwVG9rZW49MzVlOTA3MzQtYzI4Yy00ODQzLTgwNGUtOWU1MmUyZTI4MDdmCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by54YW1hcmluLmNhcnRvbWFwCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1hbmRyb2lkLTQuKgp3YXRlcm1hcms9Y3VzdG9tCg==";
		public const string HockeyId = "2e7217323aaf4ca48f66a1518497b744";

		public MapApplication(IntPtr a, JniHandleOwnership b) : base (a, b)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();

			Log.ShowError = true;
			Log.ShowWarn = true;

			// Register license
			MapView.RegisterLicense(CartoLicense, ApplicationContext);

			CrashManager.Register(this, HockeyId);
		}
	}
}

