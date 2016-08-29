

using System.Collections.Generic;
using Shared.iOS;
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

		public static List<MapListRowSource> ListOfRowSources
		{
			get
			{
				List<MapListRowSource> sources = new List<MapListRowSource>();

				foreach (UIViewController controller in List)
				{
					MapListRowSource source = new MapListRowSource { Controller = controller };

					if (controller is PackageManagerController)
					{
						source.Title = (controller as PackageManagerController).Name;
						source.Description = (controller as PackageManagerController).Description;
					}
					else {
						source.Title = (controller as MapBaseController).Name;
						source.Description = (controller as MapBaseController).Description;
					}

					sources.Add(source);
				}

				return sources;
			}
		}
	}
}

