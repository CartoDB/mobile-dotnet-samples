using System;
using Java.IO;
using Android.App;
using Carto.Layers;
using Carto.Utils;
using Shared.Droid;
using Shared;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityDescription(Description =
	                     "Read data from GeoJSON and add clustered Markers (balloons with text) to the map")]
	public class ClusteredGeoJSONCapitals: BaseMapActivity
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

