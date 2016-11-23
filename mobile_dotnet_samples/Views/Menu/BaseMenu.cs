
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class BaseMenu : UIView
	{
		const double animationDuration = 0.2;

		public bool IsVisible { get { return Alpha == 1; } }

		public BaseMenu()
		{
			Alpha = 0;

			AddGestureRecognizer(new UITapGestureRecognizer(OnBackgroundTap));

			Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);
		}

		public void Show()
		{
			UIApplication.SharedApplication.KeyWindow.AddSubview(this);

			Animate(animationDuration, delegate { Alpha = 1; });
		}

		public void Hide()
		{
			Animate(animationDuration, delegate { Alpha = 0; }, delegate { RemoveFromSuperview(); });
		}

		void OnBackgroundTap()
		{
			Hide();
		}

	}
}
