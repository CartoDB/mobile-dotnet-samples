

using System.Collections.Generic;
using AdvancedMap.iOS.Sections.OfflineMap;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class Samples
	{
		public static List<Sample> List = new List<Sample>
		{
			new Sample {
				Controller = new BaseMapsController(),
				Title = "Base Maps",
                Description = "Choice between different base maps, styles, languages",
				ImageResource = "gallery/image_base_maps.png"
			},
			new Sample {
				Controller = new CustomRasterDatasourceController(),
				Title = "Custom Raster Source",
                Description = "Customized raster tile data source",
				ImageResource = "gallery/image_custom_raster.png"
			},
			new Sample {
				Controller = new GroundOverlayController(),
				Title = "Ground Overlays",
                Description = "Shows a non-tiled Bitmap on ground",
				ImageResource = "gallery/image_ground_overlays.png"
			},
			new Sample {
                Controller = new RouteSearchController(),
				Title = "Search API",
				Description = "Finds points of interest near a route",
				ImageResource = "gallery/icon_sample_route_search.png"
			},
			new Sample {
				Controller = new WmsMapController(),
				Title = "WMS Map",
                Description = "Use external WMS service for raster tile overlay",
				ImageResource = "gallery/image_wms.png"
			},
			new Sample {
				Controller = new OfflineRoutingController(),
				Title = "Offline Routing",
                Description = "Routing and Routing package download",
				ImageResource = "gallery/image_offline_routing.png"
			},
			new Sample {
				Controller = new VectorObjectEditingController(),
				Title = "Vector Object Editing",
                Description = "Shows usage of an editable vector layer",
				ImageResource = "gallery/image_object_editing.png"
			},
			new Sample {
				Controller = new OverlaysController(),
				Title = "2D & 3D Overlays",
                Description = "Shows lines, points, polygons, texts, pop-ups",
				ImageResource = "gallery/image_overlays.png"
			},
			new Sample {
				Controller = new ClusteredMarkersController(),
				Title = "Clustered Markers",
                Description = "Shows 20,000 points from bundled geojson",
				ImageResource = "gallery/image_clustered_markers.png"
			},
			new Sample {
				Controller = new BundledMapController(),
				Title = "Offline Bundled Map",
                Description = "Shows bundled MBTiles file of Rome",
				ImageResource = "gallery/image_bundled.png"
			},
			new Sample {
				Controller = new OfflineMapController(),
				Title = "Offline Map",
                Description = "Download existing map packages for offline use",
				ImageResource = "gallery/image_country_packages.png"
			},
			new Sample {
				Controller = new CaptureController(),
				Title = "Screencapture",
                Description = "Capture rendered mapView as a Bitmap",
				ImageResource = "gallery/image_screencapture.png"
			},
			new Sample {
				Controller = new CustomPopupController(),
				Title = "Custom Popup",
                Description = "Creates a Custom Popup",
				ImageResource = "gallery/image_custom_popup.png"
			},
			new Sample {
				Controller = new GpsLocationMapController(),
				Title = "GPS Location",
                Description = "Shows user GPS location on the map",
				ImageResource = "gallery/image_gps_location.png"
			},
			new Sample {
				Controller = new GeocodingController(),
				Title = "Geocoding",
                Description = "Converting addresses into geographic coordinates",
				ImageResource = "gallery/icon_sample_geocoding.png"
			},
			new Sample {
                Controller = new ReverseGeocodingController(),
				Title = "Reverse Geocoding",
                Description = "Coding of a point location to a readable address",
				ImageResource = "gallery/icon_sample_reverse_geocoding.png"
			},
			new Sample {
                Controller = new BundledUserDataController(),
				Title = "Package Data",
                Description = "Displays available CARTO Mobile packages",
				ImageResource = "gallery/icon_sample_user_data.png"
			}
		};
	}
}

