
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

			Title = Name;
		}

		protected void AddBaseLayer(CartoBaseMapStyle withStyle)
		{
			// Initialize map
			var baseLayer = new CartoOnlineVectorTileLayer(withStyle);
			MapView.Layers.Add(baseLayer);
		}
	}
}

