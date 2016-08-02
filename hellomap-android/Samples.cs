using System;
using System.Collections.Generic;

namespace CartoMobileSample
{
	public class Samples
	{
		// List of demo activities
		static List<Type> List = new List<Type>(new Type[] {
			typeof (ClusteredGeoJSONCaptitals),
			typeof (GpsLocationMap),
			typeof (MapOverlays),
			typeof (OfflineMap),
			typeof (OfflineRouting),
			typeof (OnlineMap)
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

