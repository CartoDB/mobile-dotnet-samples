
using System;
//using AdvancedMap.iOS;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class MapListCell : BaseCell
	{
		public MapListCell()
		{
			AccessibilityIdentifier = "MapListCell";

			title.Font = UIFont.FromName("Helvetica-Bold", 12);

			description.Font = UIFont.FromName("Helvetica Neue", 11);
			description.Lines = 0;
			description.LineBreakMode = UILineBreakMode.WordWrap;
			description.TextAlignment = UITextAlignment.Justified;

			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public override void Update(MapListRowSource item)
		{
			base.Update(item);
			description.Text = item.Description;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = padding;
			nfloat y = smallPadding;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = Frame.Height / 3;

			title.Frame = new CGRect(x, y, w, h);

			y += h;
			h = Frame.Height - (h + smallPadding);

			description.Frame = new CGRect(x, y, w, h);
		}
	}
}

