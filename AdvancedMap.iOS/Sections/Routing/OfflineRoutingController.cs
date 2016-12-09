using System;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Shared;

namespace AdvancedMap.iOS
{
	public class OfflineRoutingController : BaseRoutingController
	{
		public override string Name { get { return "Offline Routing"; } }

		public override string Description { get { return "Offline routing with OpenStreetMap data packages"; } }

		internal static string[] downloadablePackages = { "EE-routing", "LV-routing" };

		RoutingPackageListener PackageListener { get; set; }

		CartoPackageManager Manager { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Manager = Routing.PackageManager;

			PackageListener = new RoutingPackageListener(Manager, downloadablePackages);

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

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			PackageListener.PackageUpdated += OnPackageUpdated;

			Manager.PackageManagerListener = PackageListener;
			Manager.Start();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			PackageListener.PackageUpdated -= OnPackageUpdated;

			Manager.Stop(true);
			Manager.PackageManagerListener = null;
		}
		void OnPackageUpdated(object sender, PackageUpdateEventArgs e)
		{
			InvokeOnMainThread(() =>
			{
				Alert("Offline package downloaded: " + e.Id);
			});

		}
	}
}
