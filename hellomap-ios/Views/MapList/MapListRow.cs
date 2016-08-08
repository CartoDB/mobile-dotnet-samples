
using System;
using CoreGraphics;
using UIKit;

namespace CartoMobileSample
{
	public class MapListRow : UITableViewCell
	{
		public int Index { get; set; }

		UILabel title, description;

		public MapListRow()
		{
			title = new UILabel();
			title.Font = UIFont.FromName("Helvetica Neue", 15);

			description = new UILabel();
			description.Font = UIFont.FromName("Helvetica Neue", 12);
			description.Lines = 0;
			description.LineBreakMode = UILineBreakMode.WordWrap;
			description.TextAlignment = UITextAlignment.Justified;

			AddSubviews(title, description);
		}

		public void Update(MapBaseController item)
		{
			title.Text = item.Name;
			description.Text = item.Description;

			SelectionStyle = UITableViewCellSelectionStyle.None;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat padding = 10;

			nfloat x = padding;
			nfloat y = 0;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = Frame.Height / 3;

			title.Frame = new CGRect(x, y, w, h);

			y += h;
			h = Frame.Height - h;

			description.Frame = new CGRect(x, y, w, h);
		}
	}
}

