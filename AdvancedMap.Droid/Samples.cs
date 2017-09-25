using System;
using System.Collections.Generic;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class Samples
	{
        public static List<MapGallerySource> Items = new List<MapGallerySource>
        {
            new MapGallerySource {
                Type = typeof(BaseMapsActivity),
                Title = "Base Maps",
                Description = "Choice between different base maps, styles, languages",
                ImageResource = Resource.Drawable.image_base_maps
            },
            new MapGallerySource {
                Type = typeof(ReverseGeocodingActivity),
                Title = "Reverse Geocoding",
                Description = "Coding of a point location to a readable address",
                ImageResource = Resource.Drawable.icon_sample_reverse_geocoding
            },
			new MapGallerySource {
                Type = typeof(GeocodingActivity),
				Title = "Geocoding",
                Description = "Converting addresses into geographic coordinates",
				ImageResource = Resource.Drawable.icon_sample_geocoding
			},
            new MapGallerySource {
                Type = typeof(CustomRasterDatasourceActivity),
                Title = "Custom Raster Data Source",
                Description = "Customized raster tile data source",
                ImageResource = Resource.Drawable.image_custom_raster
            },
			new MapGallerySource {
                Type = typeof(RouteSearchActivity),
				Title = "Search API",
                Description = "Finds points of interest near a route",
                ImageResource = Resource.Drawable.icon_sample_route_search
			},
            new MapGallerySource {
                Type = typeof(GroundOverlayActivity),
                Title = "Ground Overlays",
                Description = "Shows a non-tiled Bitmap on ground",
                ImageResource = Resource.Drawable.image_ground_overlays
            },
            new MapGallerySource {
                Type = typeof(WmsMapActivity),
                Title = "WMS Map",
                Description = "Use external WMS service for raster tile overlay",
                ImageResource = Resource.Drawable.image_wms
            },
            new MapGallerySource {
                Type = typeof(OfflineRoutingActivity),
                Title = "Offline Routing",
                ImageResource = Resource.Drawable.image_offline_routing
            },
            new MapGallerySource {
                Type = typeof(OnlineRoutingActivity),
                Title = "Online Routing",
                ImageResource = Resource.Drawable.image_online_routing
            },
            new MapGallerySource {
                Type = typeof(VectorObjectEditingActvity),
                Title = "Vector Object Editing",
                Description = "Shows usage of an editable vector layer",
                ImageResource = Resource.Drawable.image_object_editing
            },
            new MapGallerySource {
                Type = typeof(OverlaysActivity),
                Title = "2D & 3D Overlays",
                Description = "Shows lines, points, polygons, texts, pop-ups and a NMLModel",
                ImageResource = Resource.Drawable.image_overlays
            },
            new MapGallerySource {
                Type = typeof(ClusteredMarkersActivity),
                Title = "Clustered Markers",
                Description = "Shows 20,000 points from bundled geojson",
                ImageResource = Resource.Drawable.image_clustered_markers
            },
            new MapGallerySource {
                Type = typeof(BundledMapActivity),
                Title = "Bundled Map",
                Description = "Offline map of Rome bundled with the app",
                ImageResource = Resource.Drawable.image_bundled
            },
            new MapGallerySource {
                Type = typeof(BasicPackageManagerActivity),
                Title = "City Package Download",
                ImageResource = Resource.Drawable.image_city_package
            },
            new MapGallerySource {
                Type = typeof(AdvancedPackageManagerActivity),
                Title = "Country Package Download",
                ImageResource = Resource.Drawable.image_country_packages
            },
            new MapGallerySource {
                Type = typeof(CaptureActivity),
                Title = "Screencapture",
                Description = "Capture rendered mapView as a Bitmap",
                ImageResource = Resource.Drawable.image_screencapture
            },
            new MapGallerySource {
                Type = typeof(CustomPopupActivity),
                Title = "Custom Popup",
                Description = "Creates a Custom Popup",
                ImageResource = Resource.Drawable.image_custom_popup
            },
            new MapGallerySource {
                Type = typeof(GpsLocationActivity),
                Title = "GPS Location",
                Description = "Shows user GPS location on the map",
                ImageResource = Resource.Drawable.image_gps_location
            },
			new MapGallerySource {
                Type = typeof(BundledPackageDataActivity),
				Title = "Package Data",
                Description = "Displays available CARTO Mobile packages",
                ImageResource = Resource.Drawable.icon_sample_user_data
			}
        };
	}
}

