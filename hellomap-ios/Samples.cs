

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
			new PinMapController(),
			//new PackageManagerController(),// Unfinished
			new RasterOverlayController(),
			new SQLController(),
			new WmsMapController()
		};
	}
}

