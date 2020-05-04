using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Carto.Layers;
using Android.Locations;
using Android.Widget;
using Carto.VectorElements;
using Carto.DataSources;
using Carto.Core;
using Carto.Styles;
using Android.Views;
using Carto.Geometry;
using Android.OS;
using System.Collections.Generic;
using System.Linq;

using Carto.Ui;
using Shared.Droid;
using Shared.Utils;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "GPS Location Map", Description = "Shows user GPS location on map.")]
	public class GpsLocationActivity : MapBaseActivity, ILocationListener
	{
		LocationManager manager;

        // Custom shared class for creating a location marker that shows accuracy
        LocationMarker marker;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);

            marker = new LocationMarker(MapView);

			if (((int)Build.VERSION.SdkInt) >= Marshmallow)
			{
				// Ask runtime location permission
				RequestLocationPermission();
			}
			else {
				// Initialize the location manager to get the current position
				InitializeLocationManager();
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			if (manager != null)
			{
				manager.RemoveUpdates(this);
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
		{
			if (requestCode == RequestCode)
			{
				if (grantResults.Length > 0 && grantResults[0] == Android.Content.PM.Permission.Granted)
				{
					InitializeLocationManager();
					return;
				}

				Finish();
			}
		}

		void RequestLocationPermission()
		{
			string fine = Android.Manifest.Permission.AccessFineLocation;
			string coarse = Android.Manifest.Permission.AccessCoarseLocation;
			RequestPermissions(new string[] { fine, coarse }, RequestCode);
		}

		void InitializeLocationManager()
		{
			manager = (LocationManager)GetSystemService(LocationService);

			foreach (string provider in manager.GetProviders(true))
			{
				manager.RequestLocationUpdates(provider, 1000, 50, this);
			}
		}

		public void OnLocationChanged(Location location)
		{
			LocationFound(location);
		}

		public void OnProviderDisabled(string provider)
		{
			Alert("Location provider disabled, bro!");
		}

		public void OnProviderEnabled(string provider)
		{
			Alert("Location provider enabled... scanning for location");
		}

		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			System.Console.WriteLine("OnStatusChanged");
		}

		void LocationFound(Location location)
		{
            marker.ShowAt(location.Latitude, location.Longitude, location.Accuracy);
		}
	}
}

