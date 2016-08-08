
using System;
using System.Collections.Generic;
using UIKit;

namespace CartoMobileSample
{
	public class MapListDataSource : UITableViewSource
	{
		static nfloat max = 80;

		const string identifier = "TaskCell";

		public static nfloat ROWHEIGHT
		{
			get
			{
				nfloat height = Device.TrueScreenHeight / 8;

				if (height > max)
				{
					return max;
				}

				return height;
			}
		}

		public EventHandler<ControllerEventArgs> MapSelected { get; set; }

		public List<MapBaseController> Items = new List<MapBaseController>();

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
			MapBaseController controller = Items[indexPath.Row];

			if (MapSelected != null) {
				MapSelected(new object(), new ControllerEventArgs { Controller = controller });
			}

			Console.WriteLine("Clicked on: " + controller.Name);
		}

		public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			MapBaseController controller = Items[indexPath.Row];

			MapListRow cell = (MapListRow)tableView.DequeueReusableCell(identifier);

			if (cell == null)
			{
				cell = new MapListRow();
			}

			cell.Update(controller);

			return cell;
		}

	}

	public class ControllerEventArgs
	{
		public MapBaseController Controller { get; set; }
	}
}

