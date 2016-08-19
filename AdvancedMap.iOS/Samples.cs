

using System.Collections.Generic;
using UIKit;

namespace AdvancedMap.iOS
{
	public class Samples
	{
		public static List<UIViewController> List = new List<UIViewController>
		{
			new ClusteredGeoJSONCapitalsController(),
			new GpsLocationMapController(),
			new OfflineRoutingController(),
			new OfflineVectorMapController(),
			new Overlays2DController(),
			new PinMapController(),
			new PackageManagerController(),
			new RasterOverlayController(),
			new SQLController(),
			new WmsMapController()
		};
	}
}

