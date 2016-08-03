using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Carto.Layers;

namespace CartoMobileSample
{
	[Activity]
	[ActivityDescription(Description = "")]
	public class MapOverlays: BaseMapActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			/// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			MapSetup.AddMapOverlays(MapView);
		}
	}
}

