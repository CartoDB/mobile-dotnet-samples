
using System;
using System.Collections.Generic;
using UIKit;

namespace CartoMobileSample
{
	public class MapListView : UITableView
	{
		public MapListDataSource DataSource { get; set; }

		public MapListView()
		{
			BackgroundColor = UIColor.FromRGB(240, 240, 240);
		}

		public void AddRows(List<MapBaseController> controllers)
		{
			DataSource = new MapListDataSource { Items = controllers };
			Source = DataSource;
		}
	}
}

