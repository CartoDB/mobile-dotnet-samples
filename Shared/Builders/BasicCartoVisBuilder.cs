using System;
using Carto.Core;
using Carto.Layers;
using Carto.Services;
using Carto.Ui;

namespace Shared
{
	public class BasicCartoVisBuilder : CartoVisBuilder
	{
		MapView mapView;

		public BasicCartoVisBuilder(MapView mapView)
		{
			this.mapView = mapView;
		}

		public override void SetCenter(MapPos mapPos)
		{
            // Translate position from WGS84 to Mercator first
			mapView.SetFocusPos(mapView.Options.BaseProjection.FromWgs84(mapPos), 1.0f);
		}

		public override void SetZoom(float zoom)
		{
			mapView.SetZoom(zoom, 1.0f);
		}

		public override void AddLayer(Layer layer, Variant attributes)
		{
			// Add the layer to the map view
			mapView.Layers.Add(layer);
		}
	}
}

