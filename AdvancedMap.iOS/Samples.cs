

using System.Collections.Generic;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class Samples
	{
		public static List<UIViewController> List = new List<UIViewController>
		{
			new HeaderController { Name = "Base maps" },
			new BaseMapsController(),

			new HeaderController { Name = "Overlay data sources" },
			new CustomRasterDatasourceController(),
			new GroundOverlayController(),
			new WmsMapController(),

			new HeaderController { Name = "Vector objects" },
			new OverlaysController(),

			new HeaderController { Name = "Offline maps" },
			new BundledMBTilesController(),
			new PackageManagerController(),

			new HeaderController { Name = "GIS" },
			new BasicEditableOverlayController(),

			new HeaderController { Name = "Other" },
			new CaptureController(),
			new CustomPopupController(),
			new ClusteredGeoJsonController(),
			new GpsLocationMapController(),
			new OfflineRoutingController(),
		};

		public static List<MapListRowSource> RowSources
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
						source.Title = (controller as BaseController).Name;
						source.Description = (controller as BaseController).Description;
					}

					sources.Add(source);
				}

				return sources;
			}
		}
	}
}

