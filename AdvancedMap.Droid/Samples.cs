using System;
using System.Collections.Generic;

namespace AdvancedMap.Droid
{  
	public class Samples
	{
		public const string IntentName = "INTENTNAME";

		public static List<Type> List = new List<Type>(new Type[] {

			typeof(BaseMapsHeader),
			typeof(BaseMapsActivity),

			typeof(OfflineMapsHeader),
			typeof(PackageManagerActivity),
			typeof(BundledMapActivity),

			typeof(OverlayDatasourcesHeader),
			typeof(CustomRasterDatasourceActivity),
			typeof(WmsMapActivity),

			typeof(VectorObjectsHeader),
			typeof(ClusteredMarkersActivity),
			typeof(OverlaysActivity),
			typeof(VectorObjectEditingActvity),

			typeof(OtherMapHeader),
			typeof(CaptureActivity),
			typeof(CustomPopupActivity),
			typeof(GpsLocationActivity),
			typeof(OfflineRoutingActivity)
		});

		public static Type FromPosition(int position) 
		{
			return List[position];
		}
	}
}

