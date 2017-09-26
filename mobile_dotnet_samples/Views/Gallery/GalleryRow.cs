using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class GalleryRow : UIView
	{
		UIImageView image;
		UILabel label;

		public Sample Source { get; private set; }

		public GalleryRow(Sample source)
		{
			Source = source;

			BackgroundColor = Colors.CartoRed;

			image = new UIImageView();

			if (source.ImageResource != null)
			{
				image.Image = UIImage.FromFile(source.ImageResource);
				image.ContentMode = UIViewContentMode.ScaleAspectFill;
				image.ClipsToBounds = true;
			}

			label = new UILabel();
			label.Text = source.Title.ToUpper();
			label.Font = UIFont.FromName("HelveticaNeue", 10);
			label.TextColor = UIColor.White;
			label.TextAlignment = UITextAlignment.Center;

			AddSubviews(image, label);

		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			AddShadow();

			nfloat padding = 5;

			nfloat labelHeight = (Frame.Height - 3 * padding) / 4;

			nfloat x = padding;
			nfloat y = padding;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = Frame.Height - labelHeight;

			image.Frame = new CGRect(x, y, w, h);

			y += h - padding / 2;
			h = labelHeight;

			label.Frame = new CGRect(x, y, w, h);
		}

		void AddShadow()
		{
			UIBezierPath path = UIBezierPath.FromRect(Bounds);
			Layer.MasksToBounds = false;
			Layer.ShadowColor = Colors.CartoNavy.CGColor;
			Layer.ShadowOffset = new CGSize(2.0f, 2.0f);
			Layer.ShadowOpacity = 0.5f;
			Layer.ShadowPath = path.CGPath;
		}
	}
}
