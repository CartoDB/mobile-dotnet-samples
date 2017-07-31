
using System;
using System.Collections.Generic;
using Foundation;
using UIKit;

namespace Shared.iOS
{
    public class PackageListSource : UITableViewSource
    {
        public const string Identifier = "PackageCell";
		
        public EventHandler<EventArgs> CellSelected;

		public readonly List<Package> Packages = new List<Package>();

        public PackageListSource()
        {
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            Package package = Packages[indexPath.Row];

            PackageCell cell = (PackageCell)tableView.DequeueReusableCell(Identifier);

			if (cell == null)
			{
                cell = new PackageCell();
			}

            cell.Update(package);

			return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Packages.Count;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return UITableView.AutomaticDimension;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var package = Packages[indexPath.Row];
            CellSelected?.Invoke(package, EventArgs.Empty);
        }
	}
}
