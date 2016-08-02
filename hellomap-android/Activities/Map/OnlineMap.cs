using System;
using Java.IO;
using Android.App;
using Carto.Layers;
using Carto.Utils;

namespace CartoMobileSample
{
	[Activity(Label = "Online Map")]
	public class OnlineMap : BaseMapActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);
		}
	}
}

