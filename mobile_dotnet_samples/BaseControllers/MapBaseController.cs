
using Carto.Layers;
using Carto.Projections;
using Carto.Ui;
using Carto.Utils;

namespace Shared.iOS
{
	public class MapBaseController : BaseController
	{
		protected MapView MapView { get; private set; }

		protected Projection BaseProjection { get; private set; }

		protected TileLayer BaseLayer { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MapView = new MapView();
			View = MapView;

			BaseProjection = MapView.Options.BaseProjection;

			// Initialize map with default base layer
			var baseLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);

			MapView.Layers.Add(baseLayer);

			Title = Name;
		}
	}
}

