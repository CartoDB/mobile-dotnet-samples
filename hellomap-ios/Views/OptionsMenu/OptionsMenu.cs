
using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace CartoMobileSample
{
	public class OptionsMenu : UIView
	{
		static nfloat BoxPadding = 20;

		const double animationDuration = 0.2;

		const int hiddenAlpha = 0;
		const int showingAlpha = 150;

		nfloat Y = 35;

		CloseButton closeButton;

		public OptionsMenu()
		{
			BackgroundColor = UIColor.FromRGBA(0, 0, 0, 150);

			Alpha = 1;

			AddGestureRecognizer(new UITapGestureRecognizer(OnBackgroundTap));

			closeButton = new CloseButton();
			AddSubview(closeButton);

			closeButton.AddGestureRecognizer(new UITapGestureRecognizer(OnBackgroundTap));
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat buttonPadding = 5;

			nfloat w = 25;
			nfloat h = w;
			nfloat y = buttonPadding;
			nfloat x = Frame.Width - (w + buttonPadding);

			closeButton.Frame = new CGRect(x, y, w, h);
		}

		public void AddItems(string title, Dictionary<string, string> items)
		{
			OptionsMenuBox box = new OptionsMenuBox(title, items);
			box.AddGestureRecognizer(new UITapGestureRecognizer(OnBoxTap));
			AddSubview(box);

			nfloat x = BoxPadding;
			nfloat y = Y;
			nfloat w = Frame.Width - 2 * BoxPadding;
			nfloat h = box.GetHeight();

			box.Frame = new CGRect(x, y, w, h);

			Y += h + BoxPadding;
		}

		public void Show()
		{
			UIView.Animate(animationDuration, delegate { Alpha = 1; });
		}

		public void Hide()
		{
			UIView.Animate(animationDuration, delegate { Alpha = 0; });
		}

		void OnBackgroundTap()
		{
			Hide();
		}

		void OnBoxTap()
		{
			// Empty GestureRecognizer so tapping on box wouldn't close the menu
		}
	}

	public class CloseButton : UIButton
	{
		UIImageView icon;

		public CloseButton()
		{
			icon = new UIImageView();
			icon.Image = UIImage.FromFile("icon_close.png");
			BackgroundColor = UIColor.Black;

			AddSubview(icon);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			Layer.CornerRadius = Frame.Width / 2;

			nfloat padding = Frame.Width / 5;

			icon.Frame = new CGRect(padding, padding, Frame.Width - 2 * padding, Frame.Height - 2 * padding);
		}
	}
}

