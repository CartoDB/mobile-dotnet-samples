
using System;
using Android.App;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityDescription(Description = "Raster layer on top of the vector base map to provide height information")]
	public class RasterOverlayActivity : VectorBaseMapActivity
	{
		const string HillsideRasterUrl = "http://tiles.wmflabs.org/hillshading/{zoom}/{x}/{y}.png";

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Initialize hillshading raster data source, better visible in mountain ranges
			HTTPTileDataSource hillsRasterTileDataSource = new HTTPTileDataSource(0, 24, HillsideRasterUrl);

			// Add persistent caching datasource, tiles will be stored locally on persistent storage
			PersistentCacheTileDataSource cachedDataSource =
					new PersistentCacheTileDataSource(hillsRasterTileDataSource, GetExternalFilesDir(null) + "/mapcache_hills.db");

			// Initialize a raster layer with the previous data source
			RasterTileLayer hillshadeLayer = new RasterTileLayer(cachedDataSource);
			// Add the previous raster layer to the map
			MapView.Layers.Add(hillshadeLayer);

			// Animate map to a nice place
			MapView.SetFocusPos(BaseProjection.FromWgs84(new MapPos(-122.4323, 37.7582)), 1);
			MapView.SetZoom(13, 1);
		}
	}
}

