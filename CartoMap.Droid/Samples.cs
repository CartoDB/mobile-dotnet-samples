using System;
using System.Collections.Generic;
using Shared.Droid;

namespace CartoMap.Droid
{
	public class Samples
	{
		public const string IntentName = "INTENTNAME";

		// List of demo activities
		public static List<Type> List = new List<Type>(new Type[] {

			typeof(CartoJSHeader),
			typeof(CountriesVisActivity),
			typeof(DotsVisActivity),
			typeof(FontsVisActivity),

			// TODO: Currently disabled, as the feature is a work in progress
			//typeof(ImportHeader),
			//typeof(TilePackagerActivity),

			typeof(MapsHeader),
			typeof(AnonymousRasterTableActivity),
			typeof(AnonymousVectorTableActivity),
			typeof(NamedMapActivity),

			typeof(SQLHeader),
			typeof(SQLServiceActivity),
				
			typeof(TorqueHeader),
			typeof(TorqueShipsActivity),
		});

		public static List<MapGallerySource> Items = new List<MapGallerySource>
		{
			new MapGallerySource {
				Type = typeof(CountriesVisActivity),
				Title = "NYCity Subway Vis",
				ImageResource = Resource.Drawable.image_viz_subway
			},
			new MapGallerySource {
				Type = typeof(DotsVisActivity),
				Title = "Predicted Store Locations",
				ImageResource = Resource.Drawable.image_viz_store
			},
			new MapGallerySource {
				Type = typeof(FontsVisActivity),
				Title = "Fonts Vis",
				ImageResource = Resource.Drawable.image_viz_fonts
			},
			new MapGallerySource {
				Type = typeof(AnonymousRasterTableActivity),
				Title = "Anonymous Raster Tile",
				ImageResource = Resource.Drawable.image_anon_raster
			},
			new MapGallerySource {
				Type = typeof(AnonymousVectorTableActivity),
				Title = "Anonymous Vector Tile",
				ImageResource = Resource.Drawable.image_anon_vector
			},
			new MapGallerySource {
				Type = typeof(NamedMapActivity),
				Title = "Named map",
				ImageResource = Resource.Drawable.image_named
			},
			new MapGallerySource {
				Type = typeof(SQLServiceActivity),
				Title = "SQL Service",
				ImageResource = Resource.Drawable.image_sql
			},
			new MapGallerySource {
				Type = typeof(TorqueShipsActivity),
				Title = "Torque Ship",
				ImageResource = Resource.Drawable.image_torque
			}	
		};

		public static Type FromPosition(int position)
		{
			return List[position];
		}
	}
}

