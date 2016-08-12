
using System;
using System.Collections.Generic;
using UIKit;

namespace CartoMobileSample
{
	public class PackageListDataSource : UITableViewSource
	{
		static nfloat ROWHEIGHT = 60;

		const string identifier = "PackageListCell";

		public List<Package> Items = new List<Package>();

		public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return ROWHEIGHT;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			Package package = Items[indexPath.Row];

			PackageListCell cell = (PackageListCell)tableView.DequeueReusableCell(identifier);

			if (cell == null)
			{
				cell = new PackageListCell();
			}

			cell.Update(package);

			return cell;
		}
	}
}

