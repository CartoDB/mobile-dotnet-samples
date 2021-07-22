
using System;
using UIKit;

namespace Shared.iOS
{
    public class PopupSwitchButton : PopupButton
    {
        public EventHandler<EventArgs> Switched;

		UIImage image2;

        public bool IsOn
        {
            get { return imageView.Image == image; }
        }

        public PopupSwitchButton(string url1, string url2) : base(url1)
        {
            image2 = UIImage.FromFile(url2);
        }

		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
            base.TouchesEnded(touches, evt);

			if (!IsEnabled)
			{
				return;
			}

			Alpha = 1.0f;

            if (IsOn)
            {
                imageView.Image = image2;
            }
            else
            {
                imageView.Image = image;
            }

            Switched?.Invoke(this, EventArgs.Empty);
		}
    }
}
