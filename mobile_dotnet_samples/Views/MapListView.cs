
using System;
using System.Collections.Generic;
using UIKit;

namespace Shared.iOS
{
	public class MapListView : UITableView
	{
		public MapListDataSource ListSource { get; set; }

		public MapListView()
		{
			BackgroundColor = UIColor.FromRGB(240, 240, 240);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
		}

		public void AddRows(List<UIViewController> controllers)
		{
			ListSource = new MapListDataSource { Items = controllers };
			Source = ListSource;
		}

	}
}

