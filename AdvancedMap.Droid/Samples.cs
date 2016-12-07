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
			typeof(BasicPackageManagerActivity),
			typeof(AdvancedPackageManagerActivity),
			typeof(BundledMapActivity),

			typeof(RoutingHeader),
			typeof(OfflineRoutingActivity_New),
			typeof(OnlineRoutingActivity),

			typeof(OverlayDatasourcesHeader),
			typeof(CustomRasterDatasourceActivity),
			typeof(GroundOverlayActivity),
			typeof(WmsMapActivity),

			typeof(VectorObjectsHeader),
			typeof(ClusteredMarkersActivity),
			typeof(OverlaysActivity),
			typeof(VectorObjectEditingActvity),

			typeof(OtherMapHeader),
			typeof(CaptureActivity),
			typeof(CustomPopupActivity),
			typeof(GpsLocationActivity)
		});

		public static Type FromPosition(int position) 
		{
			return List[position];
		}
	}
}

