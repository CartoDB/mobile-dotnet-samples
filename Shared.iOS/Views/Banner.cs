
using System;
using UIKit;
using CoreGraphics;
using System.Timers;

namespace Shared.iOS.Views
{
    public class Banner : UIView
    {
        UIImageView image;
        UILabel label;

        public Banner()
        {
            BackgroundColor = Colors.DarkTransparentGray;

            image = new UIImageView();
            image.ContentMode = UIViewContentMode.ScaleAspectFit;
            image.ClipsToBounds = true;
            image.Image = UIImage.FromFile("icons/banner_icon_info.png");
            AddSubview(image);

            label = new UILabel();
            label.Font = UIFont.FromName("HelveticaNeue", 12);
            label.TextAlignment = UITextAlignment.Center;
            label.TextColor = UIColor.White;
            AddSubview(label);

            Alpha = 0.0f;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            nfloat padding = Frame.Height / 4;

            nfloat x = padding;
            nfloat y = padding;
            nfloat w = Frame.Height - 2 * padding;
            nfloat h = w;

            image.Frame = new CGRect(x, y, w, h);

            x += w + padding;
            y = 0;
            w = Frame.Width - (2 * x);
            h = Frame.Height;

            label.Frame = new CGRect(x, y, w, h);
        }

        void Show()
        {
            Superview.BringSubviewToFront(this);

            Animate(0.3, delegate {
                Alpha = 1.0f;   
            });    
        }

		void Hide()
		{
			Animate(0.3, delegate
			{
				Alpha = 0.0f;
			});
		}

        Timer timer;

        public void Show(string text)
        {
            Show();
            label.Text = text;

            timer = new Timer();
            timer.AutoReset = false;
            timer.Interval = 5000;

            timer.Elapsed += delegate {
                InvokeOnMainThread(delegate {
                    Hide();   
                });

                timer.Stop();
                timer = null;
            };

            timer.Start();
        }
    }
}
