
using System;
using System.Collections.Generic;
using UIKit;

namespace Shared.iOS
{
	public class GalleryDataSource : UITableViewSource
	{
		static nfloat maxRowHeight = 50;
		static nfloat headerHeight = 40;

		const string identifier = "TaskCell";

		public static nfloat RowHeight
		{
			get
			{
				nfloat height = UIScreen.MainScreen.Bounds.Size.Height / 8f;

				if (height > maxRowHeight)
				{
					return maxRowHeight;
				}

				return height;
			}
		}

		public EventHandler<ControllerEventArgs> MapSelected { get; set; }

		public List<Sample> Items = new List<Sample>();

		public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			Sample data = Items[indexPath.Row];
			if (data.IsHeader) {
				return headerHeight;
			}
			return RowHeight;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count;
		}

		public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			Sample item = Items[indexPath.Row];

			if (item.IsHeader) {
				return;
			}

			UIViewController controller = item.Controller;

			if (MapSelected != null) {
				MapSelected(new object(), new ControllerEventArgs { Controller = controller });
			}
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			Sample data = Items[indexPath.Row];

			BaseCell cell = (BaseCell)tableView.DequeueReusableCell(identifier);

			if (cell == null)
			{
				if (data.IsHeader)
				{
					cell = new MapListHeaderCell();
				}
				else {
					cell = new MapListCell();
				}
			}

			cell.Update(data);

			return cell;
		}

	}

	public class ControllerEventArgs
	{
		public UIViewController Controller { get; set; }
	}
}

