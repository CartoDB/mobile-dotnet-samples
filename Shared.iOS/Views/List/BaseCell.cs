
using System;
using UIKit;

namespace Shared.iOS
{
	public class BaseCell : UITableViewCell
	{
		public int Index { get; set; }

		protected UILabel title, description;

		protected nfloat smallPadding = 5;
		protected nfloat padding = 10;

		public BaseCell()
		{
			title = new UILabel();

			description = new UILabel();
			description.TextColor = UIColor.DarkGray;

			AddSubviews(title, description);
		}

		public virtual void Update(Sample item) 
		{
			title.Text = item.Title;
		}

	}
}

