using System;
using Carto.Core;
using Carto.Layers;
using Shared.iOS;

namespace AdvancedMap.iOS
{
    public class RouteSearchView : PackageDownloadBaseView
    {
		public VectorTileLayer BaseLayer { get; private set; }

		public RouteSearchView()
        {
			BaseLayer = AddBaseLayer(CartoBaseMapStyle.CartoBasemapStylePositron);

			var washingtonDC = Projection.FromWgs84(new MapPos(-77.0369, 38.9072));
			MapView.FocusPos = washingtonDC;
			MapView.Zoom = 14.0f;
        }
    }
}
