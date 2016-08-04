using System;
using System.Collections.Generic;

namespace CartoMobileSample
{  
	public class Samples
	{
		public const string IntentName = "INTENTNAME";

		// List of demo activities
		public static List<Type> List = new List<Type>(new Type[] {
			typeof (CartoVisJSONActivity),
			typeof (ClusteredGeoJSONCapitals),
			typeof (GpsLocationMap),
			typeof (MapOverlays),
			typeof (OfflineMap),
			typeof (OfflineRouting),
			typeof (OnlineMap),
			typeof (PinMapActivity),
			typeof (RasterOverlayActivity)
		});

		public static string[] AsStringArray {
			get {
				return List.ToStringArray();
			}
		}

		public static Type FromPosition(int position) 
		{
			return List[position];
		}
	}
}

