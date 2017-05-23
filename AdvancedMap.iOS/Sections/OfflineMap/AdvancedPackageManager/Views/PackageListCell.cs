
using System;
using Carto.PackageManager;
using CoreGraphics;
using Shared;
using UIKit;

namespace AdvancedMap.iOS
{
	public class PackageListCell : UITableViewCell
	{
		public EventHandler<EventArgs> CellActionButtonClicked;

		UILabel nameLabel, statusLabel;

		PackageManagerButton Button { get; set; }

		static nfloat MaxButtonWidth = 70;

		Package package;

		public PackageListCell()
		{
			nameLabel = new UILabel();
			nameLabel.Font = UIFont.FromName("HelveticaNeue-Bold", 15);

			statusLabel = new UILabel();
			statusLabel.Font = UIFont.FromName("HelveticaNeue", 12);
			statusLabel.TextColor = UIColor.DarkGray;

			Button = new PackageManagerButton();
			Button.TitleLabel.TextAlignment = UITextAlignment.Center;

			AddSubviews(nameLabel, statusLabel, Button);

            SelectionStyle = UITableViewCellSelectionStyle.None;

			Button.TouchUpInside += OnButtonClick;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (package == null)
			{
				return;
			}

			nfloat padding = 5;

			nfloat totalWidth = Frame.Width - 3 * padding;

			nfloat textWidth = totalWidth * 0.7f;
			nfloat buttonWidth = totalWidth - textWidth;

			nfloat buttonHeight = Frame.Height / 2;

			nfloat x = padding;
			nfloat y = 0;
			nfloat w = textWidth;
			// If package has info (subtitle), make room for it, else full height
			nfloat h = package.HasInfo ? Frame.Height / 3 * 2 : Frame.Height;

			nameLabel.Frame = new CGRect(x, y, w, h);

			y += h * 0.75f;
			h = Frame.Height - h;

			statusLabel.Frame = new CGRect(x, y, w, h);

			if (buttonWidth > MaxButtonWidth) {
				buttonWidth = MaxButtonWidth;
			}

			x = totalWidth - (buttonWidth + padding);
			y = Frame.Height / 2 - buttonHeight / 2;
			w = buttonWidth;
			h = buttonHeight;

			Button.Frame = new CGRect(x, y, w, h);
		}

		void OnButtonClick(object sender, EventArgs e)
		{
			if (CellActionButtonClicked != null)
			{
				CellActionButtonClicked(sender, e);
			}	
		}

		public string Id { get { return package.Id; } }

		public void Update(Package package)
		{
			// Local variable because it's used in Layoutsubviews
			this.package = package;

			nameLabel.Text = package.Name;

			statusLabel.Text = package.GetStatusText();

			ButtonInfo info = package.GetButtonInfo();

			Button.Text = info.Text;
			Button.Type = info.Type;
			Button.PriorityIndex = info.PriorityIndex;
			Button.PackageName = info.PackageName;
			Button.PackageId = info.PackageId;

			if (Button.Type == PMButtonType.UpdatePackages)
			{
				Button.Font = UIFont.FromName("HelveticaNeue-Bold", 14);
			}
			else {
				Button.Font = UIFont.FromName("HelveticaNeue-Light", 12);
			}
		}

	}
}

