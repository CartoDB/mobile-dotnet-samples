using System;
using Java.IO;
using Android.App;
using Carto.Layers;
using Carto.Utils;

namespace CartoMobileSample
{
	[Activity]
	[ActivityDescription(Description =
						 "A sample demonstrating how to read data from GeoJSON and add clustered Markers to map.\n " +
	                     "Both points from GeoJSON, and cluster markers are shown as Ballons which have dynamic texts\n \n " +
	                     "NB! Suggestions if you have a lot of points (tens or hundreds of thousands) and clusters:\n " +
	                     "1. Use Point geometry instead of Balloon or Marker\n " +
	                     "2. Instead of Balloon with text generate dynamically Point bitmap with cluster numbers\n " +
	                     "3. Make sure you reuse cluster style bitmaps. Creating new bitmap in rendering has technical cost")]
	public class ClusteredGeoJSONCaptitals: BaseMapActivity
	{
		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			// read json from assets and add to map
			string json;

			using (System.IO.StreamReader sr = new System.IO.StreamReader (Assets.Open ("capitals_3857.geojson")))
			{
				json = sr.ReadToEnd ();
			}

			MapSetup.AddJsonLayer (MapView, json);
		}

	}
}

