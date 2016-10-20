
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class MapListHeaderCell : BaseCell
	{
		public MapListHeaderCell()
		{
			title.Font = UIFont.FromName("Helvetica-Bold", 15);
			BackgroundColor = UIColor.FromRGB(245, 245, 245);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = padding;
			nfloat y = smallPadding;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = Frame.Height;

			title.Frame = new CGRect(x, y, w, h);
		}
	}
}

