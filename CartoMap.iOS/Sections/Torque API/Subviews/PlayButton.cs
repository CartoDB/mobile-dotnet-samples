using System;
using CoreGraphics;
using UIKit;

namespace CartoMap.iOS
{
	public class PlayButton : UIView
	{
		public EventHandler<EventArgs> Click;

		UIImage PlayImage = UIImage.FromFile("button_play.png");
		UIImage PauseImage = UIImage.FromFile("button_pause.png");

		UIImageView imageView;

		public bool IsPaused { get; private set; }

		public PlayButton()
		{
			imageView = new UIImageView();
			imageView.Image = PauseImage;

			BackgroundColor = UIColor.FromRGB(215, 82, 75);

			AddSubview(imageView);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			Layer.CornerRadius = Frame.Width / 2;

			nfloat padding = Frame.Width / 10;

			nfloat x = padding;
			nfloat y = padding;
			nfloat w = Frame.Width - 2 * padding;
			nfloat h = w;

			imageView.Frame = new CGRect(x, y, w, h);

			imageView.Image = PauseImage;
		}

		public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
		{
			Animate(0.1, delegate
			{
				Alpha = 0.7f;
			});
		}

		public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
		{
			Animate(0.1, delegate
			{
				Alpha = 1.0f;
			});
		}

		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
			Animate(0.1, delegate
			{
				Alpha = 1.0f;
			});

			if (Click != null)
			{
				Click(this, EventArgs.Empty);
			}
			ToggleImage();
		}

		void ToggleImage()
		{
			if (IsPaused)
			{
				imageView.Image = PauseImage;
				IsPaused = false;
			}
			else
			{
				imageView.Image = PlayImage;
				IsPaused = true;
			}
		}

		CGPoint GetPoint(UIEvent evt)
		{
			return (evt.AllTouches.AnyObject as UITouch).LocationInView(this);
		}
	}
}
