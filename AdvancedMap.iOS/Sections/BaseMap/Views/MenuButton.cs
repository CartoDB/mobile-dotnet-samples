
using System;
using UIKit;

namespace AdvancedMap.iOS
{
	public class MenuButton : UIBarButtonItem
	{
		public EventHandler<EventArgs> Click;

		UIImageView image;

		public MenuButton()
		{
			image = new UIImageView();
			image.Image = UIImage.FromFile("icon_more.png");
			image.Frame = new CoreGraphics.CGRect(0, 10, 20, 30);
			CustomView = image;

			image.AddGestureRecognizer(new UITapGestureRecognizer(OnImageClick));
		}

		void OnImageClick()
		{
			if (Click != null)
			{
				Click(new object(), EventArgs.Empty);
			}
		}
	}
}

