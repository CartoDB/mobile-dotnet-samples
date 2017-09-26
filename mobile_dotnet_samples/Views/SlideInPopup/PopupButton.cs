
using System;
using CoreGraphics;
using Shared.iOS.Views.Base;
using UIKit;

namespace Shared.iOS
{
    public class PopupButton : ClickView
    {
        protected UIImageView imageView;
        protected UIImage image;

        public PopupButton(string imageUrl)
        {
            BackgroundColor = UIColor.White;

            imageView = new UIImageView();
            imageView.ClipsToBounds = true;
            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            AddSubview(imageView);

            image = UIImage.FromFile(imageUrl);
            imageView.Image = image;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            Layer.CornerRadius = Frame.Width / 2;

            var padding = Frame.Height / 3.5f;
            imageView.Frame = new CGRect(padding, padding, Frame.Width - 2 * padding, Frame.Height - 2 * padding);

            this.AddRoundShadow();
        }

    }
}
