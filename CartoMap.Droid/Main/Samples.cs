using System;
using System.Collections.Generic;
using Shared.Droid;

namespace CartoMap.Droid
{
	public class Samples
	{
		public const string IntentName = "INTENTNAME";

		public static List<Sample> Items = new List<Sample>
		{
			new Sample {
				Type = typeof(AnonymousRasterTableActivity),
				Title = "Raster Tile",
                Description = "Anonymous raster tiles via CARTO maps service",
				ImageResource = Resource.Drawable.image_anon_raster
			},
			new Sample {
				Type = typeof(AnonymousVectorTableActivity),
				Title = "Vector Tile",
                Description = "Anonymous vector tiles via CARTO maps service",
				ImageResource = Resource.Drawable.image_anon_vector
			},
			new Sample {
				Type = typeof(NamedMapActivity),
				Title = "Indoor Map",
                Description = "Named map via CARTO maps service",
				ImageResource = Resource.Drawable.image_named
			},
			new Sample {
				Type = typeof(SQLServiceActivity),
				Title = "Large Cities",
                Description = "Largest cities via CARTO SQL Service",
				ImageResource = Resource.Drawable.image_sql
			},
			new Sample {
				Type = typeof(TorqueShipsActivity),
				Title = "Indoor torque",
                Description = "Torque map of movement in a shopping mall",
				ImageResource = Resource.Drawable.image_torque
			}	
		};
	}
}

