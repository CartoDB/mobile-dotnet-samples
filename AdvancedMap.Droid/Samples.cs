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
			typeof(BundledMBTilesActivity),

			typeof(OverlayDatasourcesHeader),
			typeof(CustomRasterDatasourceActivity),
			typeof(WmsMapActivity),

			typeof(VectorElementsHeader),
			typeof(Overlays2DActivity),

			typeof(GISHeader),
			typeof(BasicEditableOverlayActivity),

			typeof(OtherMapHeader),
			typeof(ScreenCaptureActivity),
			typeof(ClusteredGeoJSONCapitals),
			typeof(GpsLocationMap),
			typeof(OfflineRoutingActivity)
		});

		public static Type FromPosition(int position) 
		{
			return List[position];
		}
	}
}

