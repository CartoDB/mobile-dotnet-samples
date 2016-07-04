using System;
using Java.IO;
using Android.App;
using Carto.Layers;
using Carto.Utils;

namespace NutiteqSample
{
	[Activity (Label = "Online Map")]			
	public class OnlineMap: BaseMapActivity
	{
		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);

			/// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			mapView.Layers.Add(baseLayer);
		}
	}
}

