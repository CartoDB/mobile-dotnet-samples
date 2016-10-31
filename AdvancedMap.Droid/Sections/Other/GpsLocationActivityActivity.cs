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

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "GPS Location Map", Description = "Shows user GPS location on map.")]
	public class GpsLocationActivity : MapBaseActivity, ILocationListener
	{
		/// <summary>
		/// private field with the current coordinates
		/// </summary>
		private Location _currentLocation;

		/// <summary>
		/// The "Unable to determine your location." message.
		/// </summary>
		private TextView _textViewMessage;

		/// <summary>
		/// The location manager.
		/// </summary>
		private LocationManager _locationManager;

		/// <summary>
		/// The location provider.
		/// </summary>
		private string _locationProvider;

		/// <summary>
		/// The marker dataSource
		/// </summary>
		private LocalVectorDataSource _markerDataSource;

		private bool _markerAdded;

		private Marker _currentPositionMarker;
		private BalloonPopup _currentPositionMarkerLabel;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "mainGPS" layout resource, reload MapView
			SetContentView(Resource.Layout.MainGPS);
			MapView = (MapView)FindViewById(Resource.Id.mapView);

			// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			// Bind the textViewMessage
			_textViewMessage = FindViewById<TextView>(Resource.Id.textViewMessage);

			// create layer and add object to the layer, finally add layer to the map. 
			// All overlay layers must be same projection as base layer, so we reuse it
			_markerDataSource = new LocalVectorDataSource(MapView.Options.BaseProjection);
			var _markerLayer = new VectorLayer(_markerDataSource);
			MapView.Layers.Add(_markerLayer);

			// Initialize the location manager to get the current position
			InitializeLocationManager();

		}

		protected override void OnPause()
		{
			base.OnPause();

			// Remove the update of the position to save battery
			if ((_locationManager != null) && (!String.IsNullOrEmpty(_locationProvider)))
			{
				_locationManager.RemoveUpdates(this);
			}

		}

		protected override void OnResume()
		{
			base.OnResume();

			// Request updated position
			if ((_locationManager != null) && (!String.IsNullOrEmpty(_locationProvider)))
			{
				_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
			}
		}

		void AddMarker(string currentPositionTitle, string currentPositionSubtitle, float latitude, float longitude)
		{

			// Define the location of the marker, it must be converted to base map coordinate system
			MapPos currentLocation = MapView.Options.BaseProjection.FromWgs84(new MapPos(longitude, latitude));

			// Load default market style
			MarkerStyleBuilder markerStyleBuilder = new MarkerStyleBuilder();

			// Add the label to the Marker
			_currentPositionMarker = new Marker(currentLocation, markerStyleBuilder.BuildStyle());

			// Define label what is shown when you click on marker, with default style
			var balloonPopupStyleBuilder = new BalloonPopupStyleBuilder();
			_currentPositionMarkerLabel = new BalloonPopup(_currentPositionMarker, balloonPopupStyleBuilder.BuildStyle(), currentPositionTitle, currentPositionSubtitle);

			// Add the marker and label to the layer
			_markerDataSource.Add(_currentPositionMarker);
			_markerDataSource.Add(_currentPositionMarkerLabel);

			// Center the map in the current location
			MapView.FocusPos = currentLocation;

			// Zoom in the map in the current location
			MapView.Zoom = 19f;
		}

		void UpdateMarker(string myPosition, string subtitle, float latitude, float longitude)
		{
			if (!_markerAdded)
			{
				AddMarker(myPosition, subtitle, latitude, longitude);
				_markerAdded = true;
			}
			else {
				_currentPositionMarkerLabel.Title = myPosition;
				_currentPositionMarkerLabel.Description = subtitle;
				_currentPositionMarker.Geometry = new PointGeometry(MapView.Options.BaseProjection.FromWgs84(new MapPos(longitude, latitude)));
			}
		}

		/// <summary>
		/// Initializes the location manager.
		/// </summary>
		void InitializeLocationManager()
		{
			_locationManager = (LocationManager)GetSystemService(LocationService);

			Criteria criteria = new Criteria { Accuracy = Accuracy.Coarse };

			IList<string> acceptableProviders = _locationManager.GetProviders(criteria, true);

			if (acceptableProviders.Contains("gps"))
			{
				_locationProvider = acceptableProviders[0];
				_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);

				_textViewMessage.Text = "Using location provider: " + _locationProvider;
			}
			else
			{
				_locationProvider = string.Empty;
			}

			_textViewMessage.Visibility = ViewStates.Visible;
		}

		/// <Docs>The new location, as a Location object.</Docs>
		/// <remarks>Called when the location has changed.</remarks>
		/// <summary>
		/// Raises the location changed event.
		/// </summary>
		/// <param name="location">Location.</param>
		public void OnLocationChanged(Location location)
		{
			_currentLocation = location;
			if (_currentLocation == null)
			{
				LocationNotFound();
			}
			else
			{
				LocationFound(location);
			}
		}

		/// <Docs>the name of the location provider associated with this
		///  update.</Docs>
		/// <remarks>Called when the provider is disabled by the user. If requestLocationUpdates
		///  is called on an already disabled provider, this method is called
		///  immediately.</remarks>
		/// <format type="text/html">[Android Documentation]</format>
		/// <since version="Added in API level 1"></since>
		/// <summary>
		/// Raises the provider disabled event.
		/// </summary>
		/// <param name="provider">Provider.</param>
		public void OnProviderDisabled(string provider)
		{
			System.Console.WriteLine("OnProviderDisabled");
		}

		/// <Docs>the name of the location provider associated with this
		///  update.</Docs>
		/// <remarks>Called when the provider is enabled by the user.</remarks>
		/// <format type="text/html">[Android Documentation]</format>
		/// <since version="Added in API level 1"></since>
		/// <summary>
		/// Raises the provider enabled event.
		/// </summary>
		/// <param name="provider">Provider.</param>
		public void OnProviderEnabled(string provider)
		{
			System.Console.WriteLine("OnProviderEnabled");
		}

		/// <Docs>the name of the location provider associated with this
		///  update.</Docs>
		/// <summary>
		/// Raises the status changed event.
		/// </summary>
		/// <param name="provider">Provider.</param>
		/// <param name="status">Status.</param>
		/// <param name="extras">Extras.</param>
		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{
			System.Console.WriteLine("OnStatusChanged");
		}

		/// <summary>
		/// Add a marker in the map when a new location is found.
		/// </summary>
		/// <param name="location">Location.</param>
		void LocationFound(Location location)
		{
			string title = string.Format("Location from '{0}'", location.Provider);
			string subtitle = string.Format("lat:{0} lon:{1}", location.Latitude, location.Longitude);

			if (location.HasAccuracy)
				subtitle += string.Format("\naccuracy: {0} m", location.Accuracy);
			if (location.HasAltitude)
				subtitle += string.Format("\naltitude {0} m", location.Altitude);
			if (location.HasSpeed)
				subtitle += string.Format("\nspeed: {0} m/s", location.Speed);
			if (location.HasBearing)
				subtitle += string.Format("\nbearing: {0}", location.Bearing);


			UpdateMarker(title, subtitle, (float)location.Latitude, (float)location.Longitude);
		}

		/// <summary>
		/// Locations not found behaviour.
		/// </summary>
		void LocationNotFound()
		{
			// the error message appears o the screen
			_textViewMessage.Visibility = ViewStates.Visible;
		}

	}
}

