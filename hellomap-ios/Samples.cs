
using System;
using System.Collections.Generic;
using UIKit;

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
			new Overlays2DController()
		};
	}

}

