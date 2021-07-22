
using System;
using CoreGraphics;
using UIKit;

namespace Shared.iOS
{
	public class MenuButton : UIBarButtonItem
	{
		public EventHandler<EventArgs> Click;

		UIImageView image;

		public MenuButton(string path, CGRect frame)
		{
			image = new UIImageView();
			image.Image = UIImage.FromFile(path);
			image.Frame = frame;

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

