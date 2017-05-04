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

			nfloat padding = 5;
			int counter = 0;

			nfloat x = padding;
			nfloat y = padding;
			nfloat w = (Frame.Width - 3 * padding) / 2;
			nfloat h = w;

			foreach (GalleryRow row in rows)
			{
				bool isEven = counter % 2 == 0;

				if (!isEven)
				{
					x = w + 2 * padding;
				}
				else
				{
					x = padding;
				}

				row.Frame = new CGRect(x, y, w, h);

				if (!isEven)
				{
					y += h + padding;
				}

				counter++;

				if (counter == rows.Count)
				{
					if (rows.Count % 2 != 0)
					{
						// If row count is uneven, make sure final item fills the screen
						row.Frame = new CGRect(padding, y, Frame.Width - 2 * padding, h);

						// y is increased on odd rows, need to increase it here again if the final row is even
						y += h + padding;
					}

					// Set content size after final item has been set
					ContentSize = new CGSize(Frame.Width, y);

				}
			}

		}

		public void AddRows(List<MapListRowSource> sources)
		{
			foreach (MapListRowSource source in sources)
			{
				GalleryRow row = new GalleryRow(source);
				AddSubview(row);
				rows.Add(row);
			}
		}

	}
}
