using Carto.Ui;
using Xamarin.Forms;

namespace HelloMap.Forms
{
	public partial class App : Application
	{
#if __ANDROID__
		const string License = "XTUN3Q0ZDQ1ZEcWc0clhPNENHQVFCOTUwQzNERUhEWERBaFIrMEZ2SExxait4dFN5ZzVNU0ZGeTgxMkMzaFE9PQoKYXBwVG9rZW49MDlkMmE2ZTgtMTA0ZC00Y2U0LWI2ODEtMDBkMjQ0NGY1MDIwCnBhY2thZ2VOYW1lPWNvbS5jYXJ0by5oZWxsb21hcF9mb3JtcwpvbmxpbmVMaWNlbnNlPTEKcHJvZHVjdHM9c2RrLXhhbWFyaW4tYW5kcm9pZC00LioKd2F0ZXJtYXJrPWN1c3RvbQo=";
#elif WINDOWS_PHONE
        const string License = "XTUN3Q0ZFSEgyclNGeHJZbFN6U3NaWWM4ejZ2RW5KZ2dBaFE3SDk1UXU1cVZacWhubERvVXk0R203cVM0b2c9PQoKYXBwVG9rZW49MzJlZGFjYzEtMWExMi00Mjk3LWFhZGItNDc1MjJkYTJjNzBlCnByb2R1Y3RJZD03ZmJkNTVhMi1kOTJiLTRmOWYtODk5NS00ZjNlNWJmNWJkNDMKcHJvZHVjdHM9c2RrLXdpbnBob25lLTQuKgpvbmxpbmVMaWNlbnNlPTEKd2F0ZXJtYXJrPWN1c3RvbQo=";
#else
        const string License = "XTUMwQ0ZHMXBvYVk0cE95RHYwb05iMkNKaldkYklSdHVBaFVBeGRJamVhQzltaGZ0WlJ3bTFuVCt1cVkwSXk4PQoKYXBwVG9rZW49M2Q0MzQ0ZmYtZjczZi00NTZhLTk5YmMtOGE2ODE0MWViOTQ1CmJ1bmRsZUlkZW50aWZpZXI9Y29tLmNhcnRvLmhlbGxvbWFwLWZvcm1zCm9ubGluZUxpY2Vuc2U9MQpwcm9kdWN0cz1zZGsteGFtYXJpbi1pb3MtNC4qCndhdGVybWFyaz1jdXN0b20K";
#endif

		public App()
		{
			InitializeComponent();

            Carto.Utils.Log.ShowDebug = true;
            Carto.Utils.Log.ShowError = true;
            Carto.Utils.Log.ShowWarn = true;
            Carto.Utils.Log.ShowInfo = true;

            MainPage = new MainPage();

#if __ANDROID__
			MapView.RegisterLicense(License, Xamarin.Forms.Forms.Context);
#else
            bool result = MapView.RegisterLicense(License);
            System.Diagnostics.Debug.WriteLine(result);
#endif
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
}
}
