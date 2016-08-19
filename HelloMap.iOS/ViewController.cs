
using System;
using System.Threading.Tasks;
using Carto.Core;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Ui;
using Shared;
using UIKit;

namespace HelloMap.iOS
{
	public partial class ViewController : UIViewController
	{
		MapView MapView { get; set; }

		public ViewController(IntPtr handle) : base(handle)
		{

		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MapView = View as MapView;

			// Add base map
			CartoOnlineVectorTileLayer baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(baseLayer);

			// Set projection
			Projection projection = MapView.Options.BaseProjection;

			// Set default location and zoom
			MapPos berlin = projection.FromWgs84(new MapPos(13.38933, 52.51704));
			MapView.SetFocusPos(berlin, 0);
			MapView.SetZoom(10, 0);

			//// Load vis from URL
			string url = "http://documentation.carto.com/api/v2/viz/2b13c956-e7c1-11e2-806b-5404a6a683d5/viz.json";
			UpdateVis(url);
		}

		void UpdateVis(string url)
		{
			InvokeInBackground(delegate
			{
				MapView.Layers.Clear();

				// Create VIS loader
				CartoVisLoader loader = new CartoVisLoader();
				loader.DefaultVectorLayerMode = true;

				BasicCartoVisBuilder builder = new BasicCartoVisBuilder(MapView);

				try
				{
					loader.LoadVis(builder, url);
				}
				catch (Exception e)
				{
					Alert(e.Message);
				}

				MapPos tallinn = new MapPos(24.646469, 59.426939);

				MapView.AddMarkerToPosition(tallinn);
			});
		}

		async void Alert(string message)
		{
			// Show short android toast-like alert
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

