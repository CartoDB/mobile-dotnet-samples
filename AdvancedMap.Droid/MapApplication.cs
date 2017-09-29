
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
		const string CartoLicense = "XTUN3Q0ZCVngrbE02ZytLc2F1b1hWWkZEUC9nM21xeGNBaFJOaExoVmdOTkczOXJDRHJ5S0R0VmVTR2JFbFE9PQoKYXBwVG9rZW49OTQ5NjQ3ZDktYTc1My00ZmVkLWEwM2MtNWMyODIxOTU5YzYwCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by54YW1hcmluLmFkdmFuY2VkCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1hbmRyb2lkLTQuKgp3YXRlcm1hcms9Y3VzdG9tCg==";
		public const string HockeyId = "20ab1b68740a405595675c43783e7da0";

		public MapApplication(IntPtr a, JniHandleOwnership b) : base(a, b)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();

			Log.ShowInfo = true;
			Log.ShowError = true;
			Log.ShowWarn = true;
            Log.ShowDebug = true;

			// Register license
			MapView.RegisterLicense(CartoLicense, ApplicationContext);

			CrashManager.Register(this, HockeyId);
		}
	}

}

