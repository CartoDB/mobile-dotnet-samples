using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class MapGalleryView : UIScrollView
	{
		public EventHandler<EventArgs> RowClick;

		List<GalleryRow> rows = new List<GalleryRow>();

		public MapGalleryView()
		{
			BackgroundColor = Colors.CartoRedLight;

			AddGestureRecognizer(new UITapGestureRecognizer(OnClick));
		}

		void OnClick(UITapGestureRecognizer recognizer)
		{
			CGPoint point = recognizer.LocationInView(this);

			foreach (GalleryRow row in rows)
			{
				if (row.Frame.Contains(point))
				{
					if (RowClick != null)
					{
						RowClick(row, EventArgs.Empty);
					}	
				}
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			int itemsInRow = 2;

			if (Frame.Width > Frame.Height)
			{
				itemsInRow = 3;

				if (Frame.Width > 1000)
				{
					itemsInRow = 4;
				}
			}
			else if (Frame.Width > 700)
			{
				itemsInRow = 3;
			}

			nfloat padding = 5;
			int counter = 0;

			nfloat x = padding;
			nfloat y = padding;
			nfloat w = (Frame.Width - (itemsInRow + 1) * padding) / itemsInRow;
			nfloat h = w;

			foreach (GalleryRow row in rows)
			{
				x = (counter % itemsInRow * w) + ((counter % itemsInRow + 1) * padding);
				y = h * ((int)(counter / itemsInRow)) + padding * ((int)(counter / itemsInRow)) + padding;

				row.Frame = new CGRect(x, y, w, h);

				counter++;

				if (counter == rows.Count)
				{
					// Set content size after final item has been set
					ContentSize = new CGSize(Frame.Width, y + h + padding);
				}
			}

		}

		public void AddRows(List<Sample> sources)
		{
			foreach (Sample source in sources)
			{
				GalleryRow row = new GalleryRow(source);
				AddSubview(row);
				rows.Add(row);
			}
		}

	}
}
