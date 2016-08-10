using System;
using Carto.Core;
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;
using Carto.Utils;
using Foundation;

namespace CartoMobileSample
{
	public class MapBaseController : GLKit.GLKViewController
	{
		public virtual string Name { get; set; }

		public virtual new string Description { get; set; }

		protected CustomMapView ContentView { get; private set; }
		protected Projection BaseProjection { get; private set; }
		protected TileLayer BaseLayer { get; set; }

		protected CustomMapView MapView { get { return ContentView; } }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			ContentView = new CustomMapView();
			View = ContentView;

			Title = Name;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			BaseProjection = new EPSG3857();

			// Initialize map
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			ContentView.Layers.Add(baseLayer);
		}

		protected void Alert(string message)
		{
			// TODO alert
			Console.WriteLine("Error: " + message);
		}
	}
}

