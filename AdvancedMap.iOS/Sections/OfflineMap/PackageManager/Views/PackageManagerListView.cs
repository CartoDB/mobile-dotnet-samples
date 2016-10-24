
using System;
using System.Collections.Generic;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class PackageManagerListView : UITableView
	{
		public PackageListDataSource ListSource { get; set; }

		public PackageManagerListView()
		{
			BackgroundColor = UIColor.FromRGB(240, 240, 240);
		}

		public void AddRows(List<Package> packages)
		{
			ListSource = new PackageListDataSource { Items = packages };
			Source = ListSource;
		}
	}
}

