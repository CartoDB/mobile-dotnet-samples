using System;
using System.Threading.Tasks;
using Carto.Core;
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;
using Carto.Utils;
using Foundation;
using UIKit;

namespace CartoMobileSample
{
	public class MapBaseController : GLKit.GLKViewController
	{
		public virtual string Name { get; set; }

		public virtual new string Description { get; set; }

		protected CustomMapView MapView { get; private set; }

		protected Projection BaseProjection { get; private set; }

		protected TileLayer BaseLayer { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Use 60fps update rate, this makes zooming and panning animations much smoother
			PreferredFramesPerSecond = 60;

			MapView = new CustomMapView();
			View = MapView;

			BaseProjection = new EPSG3857();

			// Initialize map
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			Title = Name;
		}

		protected async void Alert(string message)
		{
			await ShowToast(message);
		}

		async Task ShowToast(string message, UIAlertView toast = null)
		{
			if (toast == null)
			{
				toast = new UIAlertView(null, message, null, null, null);
				toast.Show();
				await Task.Delay(1 * 1000);
				await ShowToast(message, toast);
				return;
			}

			UIView.BeginAnimations("");
			toast.Alpha = 0;
			UIView.CommitAnimations();
			toast.DismissWithClickedButtonIndex(0, true);
		}
	}
}

