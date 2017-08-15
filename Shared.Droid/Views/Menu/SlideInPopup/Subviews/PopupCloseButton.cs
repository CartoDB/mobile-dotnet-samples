
using System;
using Android.Content;
using Android.Widget;

namespace Shared.Droid
{
	public class PopupCloseButton : BaseView
	{
        ImageView image;

		public PopupCloseButton(Context context, int icon) : base(context)
		{
            image = new ImageView(context);
            image.SetImageResource(icon);
            image.SetScaleType(ImageView.ScaleType.CenterCrop);
            AddView(image);
		}

        public override void LayoutSubviews()
        {
            var padding = Frame.W / 3;
            image.SetFrame(padding, padding, Frame.W - 2 * padding, Frame.H - 2 * padding);
        }
	}
}
