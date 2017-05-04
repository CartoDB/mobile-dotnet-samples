

using System.Collections.Generic;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class Samples
	{
		//public static List<UIViewController> List = new List<UIViewController>
		//{
		//	new HeaderController { Name = "Base maps" },
		//	new BaseMapsController(),

		//	new HeaderController { Name = "Overlay data sources" },
		//	new CustomRasterDatasourceController(),
		//	new GroundOverlayController(),
		//	new WmsMapController(),

		//	new HeaderController { Name = "Routing" },
		//	new OfflineRoutingController(),
		//	new OnlineRoutingController(),

		//	new HeaderController { Name = "Vector objects" },
		//	new VectorObjectEditingController(),
		//	new OverlaysController(),
		//	new ClusteredMarkersController(),

		//	new HeaderController { Name = "Offline maps" },
		//	new BundledMapController(),
		//	new BasicPackageManagerController(),
		//	new AdvancedPackageManagerController(),

		//	new HeaderController { Name = "Other" },
		//	new CaptureController(),
		//	new CustomPopupController(),
		//	new GpsLocationMapController(),
		//};

		//public static List<MapListRowSource> RowSources
		//{
		//	get
		//	{
		//		List<MapListRowSource> sources = new List<MapListRowSource>();

		//		foreach (UIViewController controller in List)
		//		{
		//			MapListRowSource source = new MapListRowSource { Controller = controller };

		//			if (controller is AdvancedPackageManagerController)
		//			{
		//				source.Title = (controller as AdvancedPackageManagerController).Name;
		//				source.Description = (controller as AdvancedPackageManagerController).Description;
		//			}
		//			else {
		//				source.Title = (controller as BaseController).Name;
		//				source.Description = (controller as BaseController).Description;
		//			}

		//			sources.Add(source);
		//		}

		//		return sources;
		//	}
		//}

		public static List<MapListRowSource> List = new List<MapListRowSource>
		{
			new MapListRowSource {
				Controller = new BaseMapsController(),
				Title = "CARTO Base Maps"
			},
			new MapListRowSource {
				Controller = new CustomRasterDatasourceController(),
				Title = "Custom Raster Data Source"
			},
			new MapListRowSource {
				Controller = new GroundOverlayController(),
				Title = "Ground Overlays"
			},
			new MapListRowSource {
				Controller = new WmsMapController(),
				Title = "WMS Map"
			},
			new MapListRowSource {
				Controller = new OfflineRoutingController(),
				Title = "Offline Routing"
			},
			new MapListRowSource {
				Controller = new OnlineRoutingController(),
				Title = "Online Routing"
			},
			new MapListRowSource {
				Controller = new VectorObjectEditingController(),
				Title = "Vector Object Editing"
			},
			new MapListRowSource {
				Controller = new OverlaysController(),
				Title = "2D & 3D Overlays"
			},
			new MapListRowSource {
				Controller = new ClusteredMarkersController(),
				Title = "Clustered Markers"
			},
			new MapListRowSource {
				Controller = new BundledMapController(),
				Title = "Offline Bundled Map"
			},
			new MapListRowSource {
				Controller = new BasicPackageManagerController(),
				Title = "City Package Download"
			},
			new MapListRowSource {
				Controller = new AdvancedPackageManagerController(),
				Title = "Country Package Download"
			},
			new MapListRowSource {
				Controller = new CaptureController(),
				Title = "Screencapture"
			},
			new MapListRowSource {
				Controller = new CustomPopupController(),
				Title = "Custom Popup"
			},
			new MapListRowSource {
				Controller = new GpsLocationMapController(),
				Title = "GPS Location"
			}
		};
	}
}

