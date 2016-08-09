
using System;
using Carto.Layers;
using Carto.Utils;

namespace CartoMobileSample
{
	public class Overlays2DController : MapBaseController
	{
		public override string Name { get { return "2D Overlays"; } }

		public override string Description
		{
			get
			{
				return "\"A sample demonstrating how to add basic 2D objects to the map: " 
					+ "lines, points, polygon with hole, texts and pop-ups.";
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			ContentView.Layers.Add(baseLayer);

			MapSetup.AddMapOverlays(ContentView);
		}
	}
}

