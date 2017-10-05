using System;
using System.Collections.Generic;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class Samples
	{
        public static List<Sample> Items = new List<Sample>
        {
            new Sample {
                Type = typeof(BaseMapsActivity),
                Title = "Base Maps",
                Description = "Choice between different base maps, styles, languages",
                ImageResource = Resource.Drawable.image_base_maps
            },
            new Sample {
                Type = typeof(ReverseGeocodingActivity),
                Title = "Reverse Geocoding",
                Description = "Coding of a point location to a readable address",
                ImageResource = Resource.Drawable.icon_sample_reverse_geocoding
            },
			new Sample {
                Type = typeof(GeocodingActivity),
				Title = "Geocoding",
                Description = "Converting addresses into geographic coordinates",
				ImageResource = Resource.Drawable.icon_sample_geocoding
			},
            new Sample {
                Type = typeof(CustomRasterDatasourceActivity),
                Title = "Custom Raster Data",
                Description = "Customized raster tile data source",
                ImageResource = Resource.Drawable.image_custom_raster
            },
			new Sample {
                Type = typeof(RouteSearchActivity),
				Title = "Search API",
                Description = "Finds points of interest near a route",
                ImageResource = Resource.Drawable.icon_sample_route_search
			},
            new Sample {
                Type = typeof(GroundOverlayActivity),
                Title = "Ground Overlays",
                Description = "Shows a non-tiled Bitmap on ground",
                ImageResource = Resource.Drawable.image_ground_overlays
            },
            new Sample {
                Type = typeof(WmsMapActivity),
                Title = "WMS Map",
                Description = "Use external WMS service for raster tile overlay",
                ImageResource = Resource.Drawable.image_wms
            },
            new Sample {
                Type = typeof(OfflineRoutingActivity),
                Title = "Offline Routing",
                Description = "Routing package download",
                ImageResource = Resource.Drawable.image_offline_routing
            },
            new Sample {
                Type = typeof(VectorObjectEditingActvity),
                Title = "Vector Object Editing",
                Description = "Shows usage of an editable vector layer",
                ImageResource = Resource.Drawable.image_object_editing
            },
            new Sample {
                Type = typeof(OverlaysActivity),
                Title = "2D & 3D Overlays",
                Description = "Shows lines, points, polygons, texts, pop-ups and a NMLModel",
                ImageResource = Resource.Drawable.image_overlays
            },
            new Sample {
                Type = typeof(ClusteredMarkersActivity),
                Title = "Clustered Markers",
                Description = "Shows 20,000 points from bundled geojson",
                ImageResource = Resource.Drawable.image_clustered_markers
            },
            new Sample {
                Type = typeof(BundledMapActivity),
                Title = "Bundled Map",
                Description = "Offline map of Rome bundled with the app",
                ImageResource = Resource.Drawable.image_bundled
            },
            new Sample {
                Type = typeof(OfflineMapActivity),
                Title = "Offline Map",
                Description = "Map package download",
                ImageResource = Resource.Drawable.image_city_package
            },
            new Sample {
                Type = typeof(CaptureActivity),
                Title = "Screencapture",
                Description = "Capture rendered mapView as a Bitmap",
                ImageResource = Resource.Drawable.image_screencapture
            },
            new Sample {
                Type = typeof(CustomPopupActivity),
                Title = "Custom Popup",
                Description = "Creates a Custom Popup",
                ImageResource = Resource.Drawable.image_custom_popup
            },
            new Sample {
                Type = typeof(GpsLocationActivity),
                Title = "GPS Location",
                Description = "Shows user GPS location on the map",
                ImageResource = Resource.Drawable.image_gps_location
            },
			new Sample {
                Type = typeof(BundledPackageDataActivity),
				Title = "Package Data",
                Description = "Displays available CARTO Mobile packages",
                ImageResource = Resource.Drawable.icon_sample_user_data
			}
        };
	}
}

