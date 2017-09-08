
using System;
using Carto.PackageManager;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
    public class PackageCell : UITableViewCell
    {
        public Package Package { get; private set; }

        UILabel title, subtitle, statusIndicator;
        UIImageView forwardIcon;
        UIView progressIndicator;

		public PackageCell(IntPtr handle) : base (handle)
        {
            Initialize();
		}

        public PackageCell()
        {
            Initialize();
        }

        void Initialize()
        {
			SelectionStyle = UITableViewCellSelectionStyle.None;

			var titleFont = UIFont.FromName("HelveticaNeue-Bold", 13);
			var titleColor = Colors.CartoNavy;

			TextLabel.Font = titleFont;
			TextLabel.TextColor = titleColor;

			title = new UILabel();
			title.Font = titleFont;
			title.TextColor = titleColor;
			AddSubview(title);

			subtitle = new UILabel();
			subtitle.TextColor = UIColor.LightGray;
			subtitle.Font = UIFont.FromName("HelveticaNeue", 11);
			AddSubview(subtitle);

			statusIndicator = new UILabel();
			statusIndicator.TextAlignment = UITextAlignment.Center;
			statusIndicator.TextColor = Colors.AppleBlue;
			statusIndicator.Font = UIFont.FromName("HelveticaNeue-Bold", 11);
			statusIndicator.Layer.CornerRadius = 5.0f;
			statusIndicator.Layer.BorderColor = statusIndicator.TextColor.CGColor;
			AddSubview(statusIndicator);

			forwardIcon = new UIImageView();
			forwardIcon.Image = UIImage.FromFile("icons/icon_forward_blue.png");
			AddSubview(forwardIcon);

			progressIndicator = new UIView();
			progressIndicator.BackgroundColor = Colors.AppleBlue;
			AddSubview(progressIndicator);
        }

        static readonly nfloat leftPadding = 15.0f;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            nfloat x, y, w, h;

            if (Package.IsGroup)
            {
                title.Frame = CGRect.Empty;
                subtitle.Frame = CGRect.Empty;
                statusIndicator.Frame = CGRect.Empty;
                progressIndicator.Frame = CGRect.Empty;

                h = Frame.Height / 3;
                w = h / 2;
                x = Frame.Width - (w + leftPadding);
                y = Frame.Height / 2 - h / 2;

                forwardIcon.Frame = new CGRect(x, y, w, h);
                return;
            }

            title.SizeToFit();
            subtitle.SizeToFit();
            statusIndicator.SizeToFit();

            var topPadding = (Frame.Height - (title.Frame.Height + subtitle.Frame.Height)) / 2;
            var titleWidth = Frame.Width * 0.66f;

            x = leftPadding;
            y = topPadding;
            w = titleWidth;
            h = title.Frame.Height;

            title.Frame = new CGRect(x, y, w, h);

            y += h;

            subtitle.Frame = new CGRect(x, y, w, h);

            w = 82;
            h = Frame.Height / 3 * 2;
            x = Frame.Width - (w + leftPadding);
            y = Frame.Height / 2 - h / 2;

            statusIndicator.Frame = new CGRect(x, y, w, h);

            x = progressIndicator.Frame.X;
            y = progressIndicator.Frame.Y;
            w = progressIndicator.Frame.Width;
            h = progressIndicator.Frame.Height;

            progressIndicator.Frame = new CGRect(x, y, w, h);
        }

        public void Update(Package package)
        {
            Package = package;

            if (package.IsGroup)
            {
                // It's a package group. These are displayed with a single label
                TextLabel.Text = package.Name.ToUpper();
                forwardIcon.Hidden = false;
                return;
			}

            forwardIcon.Hidden = true;

            // "Hide" the original label, as these aren't used in advanced cells
            TextLabel.Text = "";

            title.Text = package.Name.ToUpper();
            subtitle.Text = package.GetStatusText();

            string action = package.ActionText;
            statusIndicator.Text = action;

            if (action == Package.ACTION_DOWNLOAD)
            {
                statusIndicator.Layer.BorderWidth = 1;
            }
            else
            {
                statusIndicator.Layer.BorderWidth = 0;
            }

            if (Package.Status == null)
            {
                progressIndicator.Frame = CGRect.Empty;
            }
            else if (Package.Status.CurrentAction != PackageAction.PackageActionDownloading)
            {
                progressIndicator.Frame = CGRect.Empty;
            }
		}

        public void Update(PackageStatus status)
        {
            Package.UpdateStatus(status);
            Update(Package, status.Progress);
        }

        public void Update(Package package, float progress)
        {
            Update(package);
            UpdateProgress(progress);
        }

        void UpdateProgress(float progress)
        {
            var width = ((Frame.Width - 2 * leftPadding) * progress) / 100;
            var height = 1;
            var y = Frame.Height - 4;

            progressIndicator.Frame = new CGRect(leftPadding, y, width, height);
        }
    }
}
