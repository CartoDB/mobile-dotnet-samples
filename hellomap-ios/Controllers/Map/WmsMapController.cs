
using System;
using Carto.Core;
using Carto.Layers;

namespace CartoMobileSample
{
	public class WmsMapController : MapBaseController
	{
		public override string Name { get { return "WMS Map"; } }

		public override string Description { get { return "WMS service raster on top of the vector base map"; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// USGS Base map: http://basemap.nationalmap.gov/arcgis/rest/services/USGSTopo/MapServer
			string url = "http://basemap.nationalmap.gov/arcgis/services/USGSTopo/MapServer/WmsServer?";
			string layers = "0";

			HttpWmsTileDataSource wms = new HttpWmsTileDataSource(0, 14, BaseProjection, false, url, "", layers, "image/png8");
			RasterTileLayer wmsLayer = new RasterTileLayer(wms);

			// Calculate zoom bias, basically this is needed to 'undo' automatic DPI scaling, 
			// we will display original raster with close to 1:1 pixel density
			double zoomLevelBias = Math.Log(MapView.Options.DPI / 160) / Math.Log(2);
			wmsLayer.ZoomLevelBias = (float)zoomLevelBias;

			MapView.Layers.Add(wmsLayer);

			// Animate map to map coverage
			MapView.SetFocusPos(BaseProjection.FromWgs84(new MapPos(-100, 40)), 1);
			MapView.SetZoom(5, 1);
		}

	}
}

