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
using Android.Support.V4.App;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "GPS Location Map", Description = "Shows user GPS location on map.")]
	public class GpsLocationActivity : MapBaseActivity, ILocationListener, ActivityCompat.IOnRequestPermissionsResultCallback
	{
		LocationManager manager;

		LocalVectorDataSource markerSource;

		bool isMarkerAdded;

		Marker positionMarker;
		BalloonPopup positionLabel;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "mainGPS" layout resource, reload MapView
			SetContentView(Resource.Layout.MainGPS);
			MapView = (MapView)FindViewById(Resource.Id.mapView);

            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);

			// Create layer and add object to the layer, finally add layer to the map. 
			// All overlay layers must be same projection as base layer, so we reuse it
			markerSource = new LocalVectorDataSource(MapView.Options.BaseProjection);
			var markerLayer = new VectorLayer(markerSource);

			MapView.Layers.Add(markerLayer);

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
			ActivityCompat.RequestPermissions(this, new string[] { fine, coarse }, RequestCode);
		}

		void AddMarker(string title, string subtitle, float latitude, float longitude)
		{
			// Define the location of the marker, it must be converted to base map coordinate system
			MapPos location = MapView.Options.BaseProjection.FromWgs84(new MapPos(longitude, latitude));

			// Load default market style
			MarkerStyleBuilder markerBuilder = new MarkerStyleBuilder();

			// Add the label to the Marker
			positionMarker = new Marker(location, markerBuilder.BuildStyle());

			// Define label what is shown when you click on marker, with default style
			var balloonBuilder = new BalloonPopupStyleBuilder();
			positionLabel = new BalloonPopup(positionMarker, balloonBuilder.BuildStyle(), title, subtitle);

			// Add the marker and label to the layer
			markerSource.Add(positionMarker);
			markerSource.Add(positionLabel);

			// Center the map in the current location
			MapView.FocusPos = location;

			// Zoom in the map in the current location
			MapView.Zoom = 19f;
		}

		void UpdateMarker(string myPosition, string subtitle, float latitude, float longitude)
		{
			if (!isMarkerAdded)
			{
				AddMarker(myPosition, subtitle, latitude, longitude);
				isMarkerAdded = true;
			}
			else 
			{
				positionLabel.Title = myPosition;
				positionLabel.Description = subtitle;
				positionMarker.Geometry = new PointGeometry(MapView.Options.BaseProjection.FromWgs84(new MapPos(longitude, latitude)));
			}
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
			// Add a marker in the map when a new location is found.

			string title = string.Format("Location from '{0}'", location.Provider);
			string subtitle = string.Format("lat:{0} lon:{1}", location.Latitude, location.Longitude);

			if (location.HasAccuracy)
			{
				subtitle += string.Format("\naccuracy: {0} m", location.Accuracy);
			}
			if (location.HasAltitude)
			{
				subtitle += string.Format("\naltitude {0} m", location.Altitude);
			}
			if (location.HasSpeed)
			{
				subtitle += string.Format("\nspeed: {0} m/s", location.Speed);
			}
			if (location.HasBearing)
			{
				subtitle += string.Format("\nbearing: {0}", location.Bearing);
			}

			UpdateMarker(title, subtitle, (float)location.Latitude, (float)location.Longitude);
		}
	}
}

