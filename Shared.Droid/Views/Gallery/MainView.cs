
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace Shared.Droid
{
	public class MainView : ScrollView
	{
		List<GalleryRow> rows;

		public EventHandler<EventArgs> RowClick;

		RelativeLayout container;

		public MainView(Context context) : base(context)
		{
            Background = new ColorDrawable(Colors.NearWhite);

			rows = new List<GalleryRow>();

			container = new RelativeLayout(context);
			container.LayoutParameters = new RelativeLayout.LayoutParams(
				RelativeLayout.LayoutParams.MatchParent, 
				RelativeLayout.LayoutParams.MatchParent
			);

			AddView(container);
		}

		void OnClick(int x, int y)
		{
			foreach (GalleryRow row in rows)
			{
				if (row.Contains(x, y))
				{
					if (RowClick != null)
					{
						RowClick(row, EventArgs.Empty);
					}
				}
			}
		}

		public void AddRows(List<Sample> sources)
		{
			int itemsInRow = 2;

			int width = Context.Resources.DisplayMetrics.WidthPixels;
			int height = Context.Resources.DisplayMetrics.HeightPixels;

			if (width > height)
			{
				itemsInRow = 3;

				if (IsLargeTablet)
				{
					itemsInRow = 4;
				}
			}
			else if (IsLargeTablet)
			{
				itemsInRow = 3;
			}

			int padding = width / 50;

			int x = padding;
			int y = padding;
			int w = (width - ((itemsInRow + 1) * padding)) / itemsInRow;
			int h = w;

			foreach (Sample source in sources)
			{
				GalleryRow row = new GalleryRow(Context, source);
				container.AddView(row);
				rows.Add(row);

                row.Frame = new CGRect(x, y, w, h);

				row.Click += (sender, e) => {
					RowClick(sender, e);
				};

				if (x == ((w * (itemsInRow - 1)) + padding * itemsInRow))
				{
					y += h + padding;
					x = padding;
				}
				else
				{
					x += w + padding;
				}

                if (sources.IndexOf(source) == sources.Count - 1)
                {
                    (row.LayoutParameters as RelativeLayout.LayoutParams).BottomMargin = padding;
                }
			}
		}

		public bool IsLargeTablet
		{
			get
			{
				int width = Context.Resources.DisplayMetrics.WidthPixels;
				int height = Context.Resources.DisplayMetrics.HeightPixels;

				int greater = height > width ? height : width;
				int lesser = height > width ? width : height;

				bool isTrue = greater > 1920 && lesser > 1080;

				if (Context.Resources.DisplayMetrics.Density > 2.5)
				{
					// If density is too large, it'll be a phone
					return false;
				}
				return isTrue;
			}
		}
	}
}
