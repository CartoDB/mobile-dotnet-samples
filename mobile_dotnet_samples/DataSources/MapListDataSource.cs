
using System;
using System.Collections.Generic;
using UIKit;

namespace Shared.iOS
{
	public class MapListDataSource : UITableViewSource
	{
		static nfloat max = 70;

		const string identifier = "TaskCell";

		public static nfloat ROWHEIGHT
		{
			get
			{
				nfloat height = UIScreen.MainScreen.Bounds.Size.Height / 8f;

				if (height > max)
				{
					return max;
				}

				return height;
			}
		}

		public EventHandler<ControllerEventArgs> MapSelected { get; set; }

		public List<MapListRowSource> Items = new List<MapListRowSource>();

		public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			return ROWHEIGHT;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UIViewController controller = Items[indexPath.Row].Controller;

			if (MapSelected != null) {
				MapSelected(new object(), new ControllerEventArgs { Controller = controller });
			}
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			MapListRowSource data = Items[indexPath.Row];

			MapListCell cell = (MapListCell)tableView.DequeueReusableCell(identifier);

			if (cell == null)
			{
				cell = new MapListCell();
			}

			cell.Update(data, indexPath.Row);

			return cell;
		}

	}

	public class ControllerEventArgs
	{
		public UIViewController Controller { get; set; }
	}
}

