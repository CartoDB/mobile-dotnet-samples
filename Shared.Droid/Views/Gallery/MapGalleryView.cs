
using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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

			container = new RelativeLayout(context);
			container.LayoutParameters = new RelativeLayout.LayoutParams(
				RelativeLayout.LayoutParams.MatchParent, 
				RelativeLayout.LayoutParams.MatchParent
			);

			AddView(container);
		}

		public override bool OnTouchEvent(Android.Views.MotionEvent e)
		{
			if (e.Action == Android.Views.MotionEventActions.Up)
			{
				OnClick((int)e.GetX(), (int)e.GetY());
			}
			return base.OnTouchEvent(e);
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
			int x = 0;
			int y = 0;
			int w = Context.Resources.DisplayMetrics.WidthPixels / 2;
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

				if (x == w)
				{
					y += h;
					x = 0;
				}
				else
				{
					x += w;
				}
			}
		}
	}
}
