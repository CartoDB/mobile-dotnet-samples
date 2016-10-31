using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Carto.Layers;
using Shared.Droid;
using Shared;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "Overlays", Description = "2D and 3D objects: lines, points, polygons, texts, pop-ups and a NMLModel")]
	public class OverlaysActivity: MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			// TODO
			base.OnCreate(savedInstanceState);

			/// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			MapSetup.AddMapOverlays(MapView);
		}
	}
}

