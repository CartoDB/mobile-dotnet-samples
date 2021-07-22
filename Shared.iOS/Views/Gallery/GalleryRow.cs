using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class GalleryRow : UIView
	{
		UIImageView image;
        UILabel title, description;

		public Sample Source { get; private set; }

		public GalleryRow(Sample source)
		{
			Source = source;

            BackgroundColor = UIColor.White;

			image = new UIImageView();

			if (source.ImageResource != null)
			{
				image.Image = UIImage.FromFile(source.ImageResource);
				image.ContentMode = UIViewContentMode.ScaleAspectFill;
				image.ClipsToBounds = true;
			}

			AddSubviews(image);

			title = new UILabel();
			title.Text = source.Title;
			title.Font = UIFont.FromName("HelveticaNeue-Bold", 14);
            title.TextColor = Colors.AppleBlue;
            AddSubview(title);

            description = new UILabel();
            description.Text = source.Description;
            description.TextColor = UIColor.LightGray;
            description.Font = UIFont.FromName("HelveticaNeue", 12);
            description.LineBreakMode = UILineBreakMode.WordWrap;
            description.Lines = 0;
            AddSubview(description);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			AddShadow();

			nfloat padding = 5;
            nfloat imageHeight = Frame.Height / 5 * 3;

            title.SizeToFit();
            description.SizeToFit();

			nfloat x = padding;
			nfloat y = padding;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = imageHeight;

			image.Frame = new CGRect(x, y, w, h);

            y += h + padding;
            h = title.Frame.Height;

			title.Frame = new CGRect(x, y, w, h);

            y += h + padding;
            h = description.Frame.Height;

            description.Frame = new CGRect(x, y, w, h);
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
