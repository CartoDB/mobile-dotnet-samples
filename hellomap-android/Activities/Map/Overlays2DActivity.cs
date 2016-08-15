using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Carto.Layers;

namespace CartoMobileSample
{
	[Activity]
	[ActivityDescription(Description = 
	                     "Demonstrates how to add basic 2D objects to the map: " +
	                     "lines, points, polygon with hole, texts and pop-ups.")]
	public class Overlays2DActivity: BaseMapActivity
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

