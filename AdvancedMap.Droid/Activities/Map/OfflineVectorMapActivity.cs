using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Shared.Droid;
using Shared;
using Carto.DataSources;
using Carto.Core;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityDescription(Description = "Uses bundled assets for the offline base map")]
	public class OfflineVectorMapActivity: VectorBaseMapActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			MapView.Options.ZoomRange = new MapRange(0, 6);
			MapView.SetZoom(3, 0);
		}

		protected override TileDataSource CreateTileDataSource()
		{
			// offline map data source
			string fileName = "world_zoom5.mbtiles";

			try
			{
				string directory = GetExternalFilesDir(null).ToString();
				string path = directory + "/" + fileName;

				Assets.CopyAssetToSDCard(fileName, path);
				Log.Debug("Copy done to " + path);

				MBTilesTileDataSource source = new MBTilesTileDataSource(0, 4, path);
				
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

