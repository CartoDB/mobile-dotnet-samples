

using System.Collections.Generic;

namespace CartoMobileSample
{
	public class Samples
	{
		public static List<MapBaseController> List = new List<MapBaseController>
		{
			new CartoVisJsonController(),
			new ClusteredGeoJSONCapitalsController(),
			new GpsLocationMapController(),
			new OfflineRoutingController(),
			new OfflineVectorMapController(),
			new Overlays2DController(),
			// Unfinished
			new PinMapController(),
			new PackageManagerController(),
			new RasterOverlayController(),
			new SQLController(),
			new WmsMapController()
		};
	}
}

