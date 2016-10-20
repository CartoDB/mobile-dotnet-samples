
using System;
using System.Threading.Tasks;
using Carto.Core;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Ui;
using Carto.VectorElements;
using Shared;
using Shared.iOS;
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

			// Set default position and zoom
			// Change projection of map so coordinates would fit on a mercator map
			MapPos berlin = MapView.Options.BaseProjection.FromWgs84(new MapPos(13.38933, 52.51704));
			MapView.SetFocusPos(berlin, 0);
			MapView.SetZoom(10, 0);

			Marker marker = MapView.AddMarkerToPosition(berlin);

			// Add simple event listener that changes size and/or color on map click
			MapView.MapEventListener = new HelloMapEventListener(marker);
		}
	}
}

