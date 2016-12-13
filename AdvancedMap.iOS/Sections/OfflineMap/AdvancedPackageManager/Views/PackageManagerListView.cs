
using System;
using System.Collections.Generic;
using Carto.PackageManager;
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

		public void Update(Package package)
		{
			foreach (UITableViewCell cell in VisibleCells)
			{
				var packageCell = (PackageListCell)cell;
				if (packageCell.Id == package.Id)
				{
					packageCell.Update(package);
				}
			}
		}

		public void Update(PackageManager manager, string id)
		{
			List<Package> packages = (Source as PackageListDataSource).Items;

			// Try to find the package that needs to be updated
			for (int i = 0; i < packages.Count; i++)
			{
				Package current = packages[i];

				if (id.Equals(current.RoutingId))
				{
					PackageStatus status = manager.GetLocalPackageStatus(id, -1);
					current.UpdateStatus(status);

					packages[i] = current;

					Update(current);
				}
			}
		}
	}
}

