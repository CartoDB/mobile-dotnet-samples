
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class MapListHeaderCell : BaseCell
	{
		const int BorderHeight = 5;

		UIView topBorder;

		public MapListHeaderCell()
		{
			title.Font = UIFont.FromName("Helvetica-Bold", 15);
			BackgroundColor = UIColor.FromRGB(245, 245, 245);

			topBorder = new UIView { BackgroundColor = UIColor.FromRGB(50, 50, 50) };
			AddSubview(topBorder);

			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = padding;
			nfloat y = 2;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = Frame.Height;

			title.Frame = new CGRect(x, y, w, h);

			topBorder.Frame = new CGRect(0, 0, Frame.Width, BorderHeight);
		}
	}
}

