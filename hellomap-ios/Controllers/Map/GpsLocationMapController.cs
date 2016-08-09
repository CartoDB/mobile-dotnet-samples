
using System;
using CoreLocation;
using UIKit;

namespace CartoMobileSample
{
	public class GpsLocationMapController : MapBaseController
	{
		public override string Name { get { return "GPS Location Map"; } }

		public override string Description
		{
			get
			{
				return "Shows user GPS location on map. " +
					"Make sure your app has location permission in Manifest file";
			}
		}

		LocationManager LocationManager { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LocationManager = new LocationManager();
			LocationManager.Start();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			LocationManager.LocationUpdated += OnLocationUpdate;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			LocationManager.LocationUpdated -= OnLocationUpdate;
		}

		void OnLocationUpdate(object sender, LocationUpdatedEventArgs e)
		{
			double latitude = e.Location.Coordinate.Latitude;
			double longitude = e.Location.Coordinate.Longitude;

			Console.WriteLine("OnLocationUpdate: " + latitude + " - " + longitude);
		}

	}
}

