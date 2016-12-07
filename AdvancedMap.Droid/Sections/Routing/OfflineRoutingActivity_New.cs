using System;
using System.IO;

using Android.App;

using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Carto.Styles;
using Carto.VectorElements;

using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Offline routing", Description = "Offline routing with OpenStreetMap data packages")]
	public class OfflineRoutingActivity_New : BaseRoutingActivity
	{       
		internal static string[] downloadablePackages = { "EE-routing", "LV-routing" };

		RoutingPackageListener PackageListener { get; set; }

		CartoPackageManager Manager { get; set; }

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Manager = Routing.PackageManager;

			PackageListener = new RoutingPackageListener(Manager, downloadablePackages);
			Manager.PackageManagerListener = PackageListener;

			Manager.Start();

			// Fetch list of available packages from server. 
			// Note that this is asynchronous operation 
			// and listener will be notified via onPackageListUpdated when this succeeds.        
			Manager.StartPackageListDownload();

			// Create offline routing service connected to package manager
			Routing.Service = new PackageManagerRoutingService(Manager);

			Alert("This sample uses an online map, but downloads routing packages");
		}

		protected override void SetBaseLayer()
		{
			AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Manager.Stop(true);
			PackageListener = null;
		}

		protected override void OnResume()
		{
			base.OnResume();

			PackageListener.PackageUpdated += OnPackageUpdated;
		}

		protected override void OnPause()
		{
			base.OnPause();

			PackageListener.PackageUpdated -= OnPackageUpdated;
		}

		void OnPackageUpdated(object sender, PackageUpdateEventArgs e)
		{
			RunOnUiThread(() =>
			{
				Alert("Offline package downloaded: " + e.Id);
			});

		}
	}
}
