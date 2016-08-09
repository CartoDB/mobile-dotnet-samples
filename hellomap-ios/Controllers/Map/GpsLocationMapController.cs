
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorElements;
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

		Marker positionMarker;
		LocalVectorDataSource markerSource;
		BalloonPopup markerLabel;

		bool IsMarkerSet { get { return positionMarker != null; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			LocationManager = new LocationManager();
			LocationManager.Start();

			// create layer and add object to the layer, finally add layer to the map. 
			// All overlay layers must be same projection as base layer, so we reuse it
			markerSource = new LocalVectorDataSource(ContentView.Options.BaseProjection);
			var _markerLayer = new VectorLayer(markerSource);
			ContentView.Layers.Add(_markerLayer);
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

			string title = "Your current location";
			string description = latitude.To4Decimals() + ", " + longitude.To4Decimals();

			MapPos location = ContentView.Options.BaseProjection.FromWgs84(new MapPos(longitude, latitude));

			if (IsMarkerSet) {
				markerLabel.Title = title;
				markerLabel.Description = description;
				positionMarker.Geometry = new PointGeometry(location);
				return;
			}

			// Load default market style
			MarkerStyleBuilder markerStyleBuilder = new MarkerStyleBuilder();

			// Add the label to the Marker
			positionMarker = new Marker(location, markerStyleBuilder.BuildStyle());

			// Define label what is shown when you click on marker, with default style
			var builder = new BalloonPopupStyleBuilder();
			markerLabel = new BalloonPopup(positionMarker, builder.BuildStyle(), title, description);

			// Add the marker and label to the layer
			markerSource.Add(positionMarker);
			markerSource.Add(markerLabel);

			// Center the map in the current location
			ContentView.FocusPos = location;

			// Zoom in the map in the current location
			ContentView.Zoom = 19f;
		}

	}
}

