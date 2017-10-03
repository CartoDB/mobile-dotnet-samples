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
				Controller = new TrainVisController(),
				Title = "Subway Vis",
                Description = "Vis.json of New York subway routes",
				ImageResource = "image_viz_subway.png",
			},
			new Sample {
				Controller = new StoresVisController(),
				Title = "Store Locations",
                Description = "Vis.json of predicted store locations",
				ImageResource = "image_viz_store.png",
			},
			new Sample {
				Controller = new AnonymousRasterTableController(),
				Title = "Raster Tile",
                Description = "Anonymous raster tiles via CARTO Maps Service",
				ImageResource = "image_anon_raster.png",
			},
			new Sample {
				Controller = new AnonymousVectorTableController(),
				Title = "Vector Tile",
                Description = "Anonymous vector tiles via CARTO Maps Service",
				ImageResource = "image_anon_vector.png",
			},
			new Sample {
				Controller = new NamedMapController(),
				Title = "Indoor Map",
                Description = "Names map via CARTO Maps Service",
				ImageResource = "image_named.png",
			},
			new Sample {
				Controller = new SQLServiceController(),
				Title = "Large Cities",
                Description = "Largest cities via CARTO Sql Service",
				ImageResource = "image_sql.png",
			},
			new Sample {
				Controller = new TorqueShipController(),
				Title = "Indoor Torque",
                Description = "Torque map of movement in a shopping mall",
				ImageResource = "image_torque.png",
			}
		};

	}
}

