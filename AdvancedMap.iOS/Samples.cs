

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
				Title = "CARTO Base Maps",
				ImageResource = "gallery/image_base_maps.png"
			},
			new MapListRowSource {
				Controller = new CustomRasterDatasourceController(),
				Title = "Custom Raster Data Source",
				ImageResource = "gallery/image_custom_raster.png"
			},
			new MapListRowSource {
				Controller = new GroundOverlayController(),
				Title = "Ground Overlays",
				ImageResource = "gallery/image_ground_overlays.png"
			},
			new MapListRowSource {
				Controller = new WmsMapController(),
				Title = "WMS Map",
				ImageResource = "gallery/image_wms.png"
			},
			new MapListRowSource {
				Controller = new OfflineRoutingController(),
				Title = "Offline Routing",
				ImageResource = "gallery/image_offline_routing.png"
			},
			new MapListRowSource {
				Controller = new OnlineRoutingController(),
				Title = "Online Routing",
				ImageResource = "gallery/image_online_routing.png"
			},
			new MapListRowSource {
				Controller = new VectorObjectEditingController(),
				Title = "Vector Object Editing",
				ImageResource = "gallery/image_object_editing.png"
			},
			new MapListRowSource {
				Controller = new OverlaysController(),
				Title = "2D & 3D Overlays",
				ImageResource = "gallery/image_overlays.png"
			},
			new MapListRowSource {
				Controller = new ClusteredMarkersController(),
				Title = "Clustered Markers",
				ImageResource = "gallery/image_clustered_markers.png"
			},
			new MapListRowSource {
				Controller = new BundledMapController(),
				Title = "Offline Bundled Map",
				ImageResource = "gallery/image_bundled.png"
			},
			new MapListRowSource {
				Controller = new BasicPackageManagerController(),
				Title = "City Package Download",
				ImageResource = "gallery/image_city_package.png"
			},
			new MapListRowSource {
				Controller = new AdvancedPackageManagerController(),
				Title = "Country Package Download",
				ImageResource = "gallery/image_country_packages.png"
			},
			new MapListRowSource {
				Controller = new CaptureController(),
				Title = "Screencapture",
				ImageResource = "gallery/image_screencapture.png"
			},
			new MapListRowSource {
				Controller = new CustomPopupController(),
				Title = "Custom Popup",
				ImageResource = "gallery/image_custom_popup.png"
			},
			new MapListRowSource {
				Controller = new GpsLocationMapController(),
				Title = "GPS Location",
				ImageResource = "gallery/image_gps_location.png"
			},
			new MapListRowSource {
				Controller = new GeocodingController(),
				Title = "GEOCODING",
				ImageResource = "gallery/icon_sample_geocoding.png"
			},
			new MapListRowSource {
                Controller = new ReverseGeocodingController(),
				Title = "REVERSE GEOCODING",
				ImageResource = "gallery/icon_sample_reverse_geocoding.png"
			}
		};
	}
}

