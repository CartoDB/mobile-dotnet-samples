
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class RasterOverlayController : MapBaseController
	{
		const string HillsideRasterUrl = "http://tiles.wmflabs.org/hillshading/{zoom}/{x}/{y}.png";

		public override string Name { get { return "Raster Overlay"; } }

		public override string Description { 
			get {
				return "Raster layer on top of the vector base map to provide height information";
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Initialize hillshading raster data source, better visible in mountain ranges
			HTTPTileDataSource tileSource = new HTTPTileDataSource(0, 24, HillsideRasterUrl);

			// Add persistent caching datasource, tiles will be stored locally on persistent storage
			PersistentCacheTileDataSource cachedSource =
				new PersistentCacheTileDataSource(tileSource, Utils.GetDocumentDirectory() + "mapcache_hills.db");

			// Initialize a raster layer with the previous data source
			RasterTileLayer hillshadeLayer = new RasterTileLayer(cachedSource);
			// Add the previous raster layer to the map
			MapView.Layers.Add(hillshadeLayer);

			// Animate map to a nice place
			MapView.SetFocusPos(BaseProjection.FromWgs84(new MapPos(-122.4323, 37.7582)), 1);
			MapView.SetZoom(13, 1);
		}

	}
}