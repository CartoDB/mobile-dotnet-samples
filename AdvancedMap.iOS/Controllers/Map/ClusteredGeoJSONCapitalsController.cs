
using System;
using Carto.Layers;
using Carto.Utils;
using Shared;

namespace AdvancedMap.iOS
{
	public class ClusteredGeoJSONCapitalsController : MapBaseController
	{
		public override string Name { get { return "Clustered GeoJson Capitals"; } }

		public override string Description
		{
			get
			{
				return "Read data from GeoJSON and add clustered Markers (balloons with text) to the map";
			}
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			// read json from assets and add to map
			var json = System.IO.File.ReadAllText(AssetUtils.CalculateResourcePath("capitals_3857.geojson"));

			MapSetup.AddJsonLayer(MapView, json);
		}
	}
}

