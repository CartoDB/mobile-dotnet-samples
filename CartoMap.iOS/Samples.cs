using System;
using System.Collections.Generic;
using Shared.iOS;
using UIKit;

namespace CartoMap.iOS
{
	public class Samples
	{
		public static List<UIViewController> List = new List<UIViewController>
		{
			new HeaderController() { Name = "CARTO.js API" },
			new TrainVisController(),
			new StoresVisController(),
			new FontsVisController(),
			// TODO: Currently disabled, as the feature is a work in progress
			//new HeaderController() { Name = "Import API" },
			//new TilePackagerController(),
			new HeaderController() { Name = "Maps API" },
			new AnonymousRasterTableController(),
			new AnonymousVectorTableController(),
			new NamedMapController(),
			new HeaderController() { Name = "SQL API" },
			new SQLServiceController(),
			new HeaderController() { Name = "Torque API" },
			new TorqueShipController()
		};

		public static List<MapListRowSource> RowSources
		{
			get
			{
				List<MapListRowSource> sources = new List<MapListRowSource>();

				foreach (UIViewController controller in List)
				{
					MapListRowSource source = new MapListRowSource { Controller = controller };

					source.Title = (controller as BaseController).Name;
					source.Description = (controller as BaseController).Description;

					sources.Add(source);
				}

				return sources;
			}
		}
	}
}

