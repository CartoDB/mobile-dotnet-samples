using System;
using System.Collections.Generic;
using Shared.iOS;
using UIKit;

namespace CartoMap.iOS
{
	public class Samples
	{
		public static List<Sample> List = new List<Sample>
		{
			new Sample {
				Controller = new AnonymousRasterTableController(),
				Title = "Raster Map in CARTO",
                Description = "Raster tiles from CARTO Maps API",
				ImageResource = "image_anon_raster.png",
			},
			new Sample {
				Controller = new AnonymousVectorTableController(),
				Title = "Vector Tile",
                Description = "Anonymous vector tiles via CARTO Maps API",
				ImageResource = "image_anon_vector.png",
			},
			new Sample {
				Controller = new NamedMapController(),
				Title = "Indoor Map",
                Description = "Interactive indoor map via CARTO Maps API",
				ImageResource = "image_named.png",
			},
			new Sample {
				Controller = new SQLServiceController(),
				Title = "Large Cities",
                Description = "Largest cities via CARTO SQL API",
				ImageResource = "image_sql.png",
			},
			new Sample {
				Controller = new TorqueShipController(),
				Title = "Indoor Animation",
                Description = "Torque map with indoor positioning in a shopping mall",
				ImageResource = "image_torque.png",
			}
		};

	}
}

