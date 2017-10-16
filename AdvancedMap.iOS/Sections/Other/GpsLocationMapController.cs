
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorElements;
using CoreLocation;
using Shared;
using Shared.iOS;
using Shared.Utils;
using UIKit;

namespace AdvancedMap.iOS
{
	public class GpsLocationMapController : MapBaseController
	{
		public override string Name { get { return "GPS Location Map"; } }

		public override string Description { get { return "Shows user GPS location on map"; } }

        LocationManager manager;

        LocationMarker marker;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

            // Add default base layer
            AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
			manager = new LocationManager();

            marker = new LocationMarker(MapView);
		}	

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

            manager.Start();
			manager.LocationUpdated += OnLocationUpdate;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			manager.LocationUpdated -= OnLocationUpdate;
		}

		void OnLocationUpdate(object sender, LocationUpdatedEventArgs e)
		{
            var coordinate = e.Location.Coordinate;
            var accuracy = (float)e.Location.HorizontalAccuracy;
            marker.ShowAt(coordinate.Latitude, coordinate.Longitude, accuracy);
		}

	}
}

