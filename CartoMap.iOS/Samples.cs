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
			new CartoVisJsonController()
		};

		public static List<MapListRowSource> ListOfRowSources
		{
			get
			{
				List<MapListRowSource> sources = new List<MapListRowSource>();

				foreach (UIViewController controller in List)
				{
					MapListRowSource source = new MapListRowSource { Controller = controller };

					source.Title = (controller as MapBaseController).Name;
					source.Description = (controller as MapBaseController).Description;

					sources.Add(source);
				}

				return sources;
			}
		}
	}
}

