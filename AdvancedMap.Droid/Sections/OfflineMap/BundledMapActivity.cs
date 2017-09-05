using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Shared.Droid;
using Shared;
using Carto.DataSources;
using Carto.Core;
using Carto.VectorTiles;
using Carto.Layers;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Bundled map", Description = "Offline map of Rome bundled with the app")]
	public class BundledMapActivity: MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Remove default baselayer
			MapView.Layers.Clear();

            var decoder = CartoVectorTileLayer.CreateTileDecoder(CartoBaseMapStyle.CartoBasemapStyleVoyager);

			// Do the actual copying and source creation on another thread so it wouldn't block the main thread
			System.Threading.Tasks.Task.Run(delegate
			{
				TileDataSource source = FileUtils.CreateTileDataSource(this, "rome_cartostreets.mbtiles");

				var layer = new VectorTileLayer(source, decoder);

				RunOnUiThread(delegate
				{
					// However, actual layer insertion should be done on the main thread
					MapView.Layers.Insert(0, layer);
				});
			});

			// Zoom to the correct location
			MapPos rome = BaseProjection.FromWgs84(new MapPos(12.4807, 41.8962));
			MapView.SetFocusPos(rome, 0);
			MapView.SetZoom(13, 0);
		}

	}
}

