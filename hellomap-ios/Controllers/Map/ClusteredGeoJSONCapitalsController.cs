
using System;
using Carto.Layers;
using Carto.Utils;

namespace CartoMobileSample
{
	public class ClusteredGeoJSONCapitalsController : MapBaseController
	{
		public override string Name { get { return "Clustered GeoJson Capitals"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to read data from GeoJSON and add clustered Markers to map. " +
						"Both points from GeoJSON, and cluster markers are shown as Ballons that have dynamic texts.";
			}
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			ContentView.Layers.Add(baseLayer);

			// read json from assets and add to map
			var json = System.IO.File.ReadAllText(AssetUtils.CalculateResourcePath("capitals_3857.geojson"));

			MapSetup.AddJsonLayer(ContentView, json);
		}
	}
}

