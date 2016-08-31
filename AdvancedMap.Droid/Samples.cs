using System;
using System.Collections.Generic;

namespace AdvancedMap.Droid
{  
	public class Samples
	{
		public const string IntentName = "INTENTNAME";

		// List of demo activities
		public static List<Type> List = new List<Type>(new Type[] {
			typeof (ClusteredGeoJSONCapitals),
			typeof (GpsLocationMap),
			typeof (Overlays2DActivity),
			typeof (OfflineVectorMapActivity),
			typeof (OfflineRoutingActivity),
			typeof (PinMapActivity),
			typeof (PackageManagerActivity),
			typeof (RasterOverlayActivity),
			typeof (WmsMapActivity)
		});

		public static Type FromPosition(int position) 
		{
			return List[position];
		}
	}
}

