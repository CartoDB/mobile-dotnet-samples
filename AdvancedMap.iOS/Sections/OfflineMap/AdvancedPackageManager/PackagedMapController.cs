using System;
using Carto.Layers;
using Carto.PackageManager;
using Shared.iOS;

namespace AdvancedMap.iOS
{
	public class PackagedMapController : MapBaseController
	{
		CartoPackageManager manager;

		public PackagedMapController(CartoPackageManager manager)
		{
			this.manager = manager;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = "Packaged map";

			var layer = new CartoOfflineVectorTileLayer(manager, CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(layer);
		}
	}
}
