
using System;
using CoreGraphics;
using Shared.iOS;
using Shared.iOS.Views.Base;
using UIKit;
namespace AdvancedMap.iOS.Sections.BaseMap.Subviews
{
    public class StylePopupContentSectionItem : ClickView
    {
		UIImageView imageView;
		public UILabel Label { get; private set; }

        nfloat borderWidth;

		public StylePopupContentSectionItem(string text, string resource)
		{
            BackgroundColor = UIColor.White;

			imageView = new UIImageView();
            imageView.ClipsToBounds = true;
            imageView.Image = UIImage.FromFile(resource);
			AddSubview(imageView);

			Label = new UILabel();
			Label.Text = text;
            Label.TextColor = Colors.AppleBlue;
            Label.Font = UIFont.FromName("HelveticaNeue", 11.0f);
			AddSubview(Label);

            Layer.BorderColor = Colors.AppleBlue.CGColor;

            borderWidth = 2;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

            nfloat padding = 5;

			nfloat x = borderWidth;
			nfloat y = borderWidth;
			nfloat w = Frame.Width - 2 * borderWidth;
			nfloat h = (Frame.Height - 2 * borderWidth) / 3 * 2;

			imageView.Frame = new CGRect(x, y, w, h);

            Label.SizeToFit();

			y += h + padding;
            h = Label.Frame.Height;

            Label.Frame = new CGRect(x, y, w, h);
		}

		public void Highlight()
		{
            Layer.BorderWidth = borderWidth;
		}

		public void Normalize()
		{
            Layer.BorderWidth = 0;
		}
    }
}
