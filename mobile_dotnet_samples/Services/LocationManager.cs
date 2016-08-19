
using System;
using CoreLocation;
using UIKit;

namespace Shared.iOS
{
	public class LocationManager
	{
		public event EventHandler<LocationUpdatedEventArgs> LocationUpdated;

		public CLLocationManager Manager { get; private set; }

		public LocationManager()
		{
			this.Manager = new CLLocationManager();
			this.Manager.PausesLocationUpdatesAutomatically = false;

			// iOS 8 has additional permissions requirements
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				Manager.RequestAlwaysAuthorization();
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
			{
				Manager.AllowsBackgroundLocationUpdates = true;
			}
		}

		public void Start()
		{
			if (CLLocationManager.LocationServicesEnabled)
			{
				// In meters
				Manager.DesiredAccuracy = 1;

				Manager.LocationsUpdated += OnLocationUpdated;

				Manager.StartUpdatingLocation();
			}
		}

		void OnLocationUpdated(object sender, CLLocationsUpdatedEventArgs e)
		{
			if (LocationUpdated != null)
			{
				CLLocation location = e.Locations[e.Locations.Length - 1];
				LocationUpdated(this, new LocationUpdatedEventArgs { Location = location });
			}
		}

	}

	public class LocationUpdatedEventArgs : EventArgs
	{
		public CLLocation Location { get; set; }
	}
}

