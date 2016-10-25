
using System;
using Android.App;
using Android.Runtime;
using Carto.Ui;
using Carto.Utils;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Application]
	public class MapApplication : Application
	{
		const string LICENSE = "XTUN3Q0ZFMmhzUWEwdlZwNlJNVW5kL1hMRExMYXNYVUxBaFFhKzlSK2drYjEzNnBhSTJzY1c5aDBkL2licFE9PQoKcHJvZHVjdHM9c2RrLXhhbWFyaW4tYW5kcm9pZC00LioKcGFja2FnZU5hbWU9Y29tLmNhcnRvLmFkdmFuY2VkbWFwLnhhbWFyaW4uZHJvaWQKd2F0ZXJtYXJrPWRldmVsb3BtZW50CnZhbGlkVW50aWw9MjAxNi0wOS0xOApvbmxpbmVMaWNlbnNlPTEK";

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
			MapView.RegisterLicense(LICENSE, ApplicationContext);
		}
	}

}

