using System;
using System.Collections.Generic;
using Shared.iOS;
using UIKit;

namespace CartoMap.iOS
{
	public class Samples
	{
		//public static List<UIViewController> List = new List<UIViewController>
		//{
		//	new HeaderController() { Name = "CARTO.js API" },
		//	new TrainVisController(),
		//	new StoresVisController(),
		//	new FontsVisController(),
		//	// TODO: Currently disabled, as the feature is a work in progress
		//	//new HeaderController() { Name = "Import API" },
		//	//new TilePackagerController(),
		//	new HeaderController() { Name = "Maps API" },
		//	new AnonymousRasterTableController(),
		//	new AnonymousVectorTableController(),
		//	new NamedMapController(),
		//	new HeaderController() { Name = "SQL API" },
		//	new SQLServiceController(),
		//	new HeaderController() { Name = "Torque API" },
		//	new TorqueShipController()
		//};

		//public static List<MapListRowSource> RowSources
		//{
		//	get
		//	{
		//		List<MapListRowSource> sources = new List<MapListRowSource>();

		//		foreach (UIViewController controller in List)
		//		{
		//			MapListRowSource source = new MapListRowSource { Controller = controller };

		//			source.Title = (controller as BaseController).Name;
		//			source.Description = (controller as BaseController).Description;

		//			sources.Add(source);
		//		}

		//		return sources;
		//	}
		//}


		public static List<Sample> List = new List<Sample>
		{
			new Sample {
				Controller = new TrainVisController(),
				Title = "NYCity Subway Vis",
				ImageResource = "image_viz_subway.png",
			},
			new Sample {
				Controller = new StoresVisController(),
				Title = "Predicted Store Locations",
				ImageResource = "image_viz_store.png",
			},
			new Sample {
				Controller = new FontsVisController(),
				Title = "Fonts Vis",
				ImageResource = "image_viz_fonts.png",
			},
			new Sample {
				Controller = new AnonymousRasterTableController(),
				Title = "Anonymous Raster Tile",
				ImageResource = "image_anon_raster.png",
			},
			new Sample {
				Controller = new AnonymousVectorTableController(),
				Title = "Anonymous Vector Tile",
				ImageResource = "image_anon_vector.png",
			},
			new Sample {
				Controller = new NamedMapController(),
				Title = "Named map",
				ImageResource = "image_named.png",
			},
			new Sample {
				Controller = new SQLServiceController(),
				Title = "SQL Service",
				ImageResource = "image_sql.png",
			},
			new Sample {
				Controller = new TorqueShipController(),
				Title = "Indoor Torque",
				ImageResource = "image_torque.png",
			}
		};

	}
}

