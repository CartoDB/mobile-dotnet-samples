
using System;
using Android.App;
using Android.Runtime;
using Carto.Ui;
using Carto.Utils;
using HockeyApp.Android;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Application]
	public class MapApplication : Application
	{
		const string CartoLicense = "XTUM0Q0ZRQ3JSQmVFQ0IzbmJEV091R1RrRUxGd0F4MDREd0lWQUxoT3E0UkRrT0lrTkcweDlVMHlWRk5MNlByKwoKYXBwVG9rZW49ZTE5YzI2ZTItYzAzMS00YmE2LWFjYzgtNzRmZDIzOGE4MmNjCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by54YW1hcmluLmFkdmFuY2VkbWFwCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1hbmRyb2lkLTQuKgp3YXRlcm1hcms9Y3VzdG9tCg==";
		public const string HockeyId = "20ab1b68740a405595675c43783e7da0";

		public MapApplication(IntPtr a, JniHandleOwnership b) : base(a, b)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();

			MapBaseActivity.ViewResource = Resource.Layout.Main;
			MapBaseActivity.MapViewResource = Resource.Id.mapView;

			Log.ShowInfo = true;
			Log.ShowError = true;
			Log.ShowWarn = true;

			// Register license
			MapView.RegisterLicense(CartoLicense, ApplicationContext);

			CrashManager.Register(this, HockeyId);
		}
	}

}

