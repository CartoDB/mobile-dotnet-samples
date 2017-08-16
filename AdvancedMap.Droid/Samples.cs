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
                Title = "CARTO Base Maps",
                ImageResource = Resource.Drawable.image_base_maps
            },
            new MapGallerySource {
                Type = typeof(ReverseGeocodingActivity),
                Title = "Reverse Geocoding",
                ImageResource = Resource.Drawable.icon_sample_reverse_geocoding
            },
			new MapGallerySource {
                Type = typeof(GeocodingActivity),
				Title = "Geocoding",
				ImageResource = Resource.Drawable.icon_sample_geocoding
			},
            new MapGallerySource {
                Type = typeof(CustomRasterDatasourceActivity),
                Title = "Custom Raster Data Source",
                ImageResource = Resource.Drawable.image_custom_raster
            },
            new MapGallerySource {
                Type = typeof(GroundOverlayActivity),
                Title = "Ground Overlays",
                ImageResource = Resource.Drawable.image_ground_overlays
            },
            new MapGallerySource {
                Type = typeof(WmsMapActivity),
                Title = "WMS Map",
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
                ImageResource = Resource.Drawable.image_object_editing
            },
            new MapGallerySource {
                Type = typeof(OverlaysActivity),
                Title = "2D & 3D Overlays",
                ImageResource = Resource.Drawable.image_overlays
            },
            new MapGallerySource {
                Type = typeof(ClusteredMarkersActivity),
                Title = "Clustered Markers",
                ImageResource = Resource.Drawable.image_clustered_markers
            },
            new MapGallerySource {
                Type = typeof(BundledMapActivity),
                Title = "Offline Bundled Map",
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
                ImageResource = Resource.Drawable.image_screencapture
            },
            new MapGallerySource {
                Type = typeof(CustomPopupActivity),
                Title = "Custom Popup",
                ImageResource = Resource.Drawable.image_custom_popup
            },
            new MapGallerySource {
                Type = typeof(GpsLocationActivity),
                Title = "GPS Location",
                ImageResource = Resource.Drawable.image_gps_location
            }
        };
	}
}

