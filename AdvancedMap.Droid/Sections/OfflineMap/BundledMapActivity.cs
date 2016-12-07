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

			AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);

			TileDataSource source = CreateTileDataSource();

			// Get decoder from current layer,
			// so we wouldn't need a style asset to create a decoder from scratch
			MBVectorTileDecoder decoder = (MBVectorTileDecoder)(MapView.Layers[0] as VectorTileLayer).TileDecoder;

			// Remove default baselayer
			MapView.Layers.Clear();

			// Add our new layer
			var layer = new VectorTileLayer(source, decoder);

			MapView.Layers.Insert(0, layer);

			// Zoom to the correct location
			MapPos rome = BaseProjection.FromWgs84(new MapPos(12.4807, 41.8962));
			MapView.SetFocusPos(rome, 0);
			MapView.SetZoom(13, 0);
		}

		TileDataSource CreateTileDataSource()
		{
			// offline map data source
			string fileName = "rome_ntvt.mbtiles";

			try
			{
				string directory = GetExternalFilesDir(null).ToString();
				string path = directory + "/" + fileName;

				Assets.CopyAssetToSDCard(fileName, path);
				Log.Debug("Copy done to " + path);

				MBTilesTileDataSource source = new MBTilesTileDataSource(0, 14, path);
				
				return source;
			}
			catch (IOException e)
			{
				Log.Debug("MbTileFile cannot be copied: " + fileName);
				Log.Debug("Message" + e.LocalizedMessage);
			}

			return null;
		}
	}
}

