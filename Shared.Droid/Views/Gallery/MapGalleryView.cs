
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace Shared.Droid
{
	public class MapGalleryView : ScrollView
	{
		List<GalleryRow> rows;

		public EventHandler<EventArgs> RowClick;

		RelativeLayout container;

		public MapGalleryView(Context context) : base(context)
		{
			Background = new ColorDrawable(Colors.CartoRedLight);

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

		public void AddRows(List<MapGallerySource> sources)
		{
			int itemsInRow = 2;

			int width = Context.Resources.DisplayMetrics.WidthPixels;
			int height = Context.Resources.DisplayMetrics.HeightPixels;

			if (width > height)
			{
				itemsInRow = 3;
			}

			int padding = width / 50;

			int x = padding;
			int y = padding;
			int w = (width - ((itemsInRow + 1) * padding)) / itemsInRow;
			int h = w;

			foreach (MapGallerySource source in sources)
			{
				GalleryRow row = new GalleryRow(Context, source);
				container.AddView(row);
				rows.Add(row);

				var parameters = new RelativeLayout.LayoutParams(w, h);
				parameters.LeftMargin = x;
				parameters.TopMargin = y;

				row.LayoutParameters = parameters;

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
			}
		}
	}
}
