
using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace CartoMobileSample
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

			if (!didAddMenu) {
				// LayoutSubviews() is called before Frame has width
				AddMenu();
				didAddMenu = true;
			}
		}

		public void AddRows(List<MapBaseController> controllers)
		{
			ListSource = new MapListDataSource { Items = controllers };
			Source = ListSource;
		}

		bool didAddMenu;

		public void AddMenu()
		{
			OptionsMenu menu = new OptionsMenu();
			menu.Show();
			menu.Frame = new CGRect(0, 0, Frame.Width, Frame.Height);
			AddSubview(menu);

			menu.AddItems("Languages", new Dictionary<string, string> {
				{ "English", "en" },
				{ "German", "de" },
				{ "Spanish", "es" },
				{ "Italian", "it" },
				{ "French", "fr" },
				{ "Russian", "ru" },
				{ "Chinese", "zh" }
			});
		}

	}
}

