
using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace Shared.Droid
{
	public class PopupBackButton : BaseView
	{
        ImageView button;
        TextView text;

		public PopupBackButton(Context context, int icon) : base(context)
		{
            SetBackgroundColor(Color.White);
            Visibility = Android.Views.ViewStates.Gone;

            button = new ImageView(context);
            button.SetImageResource(icon);
            button.SetScaleType(ImageView.ScaleType.CenterCrop);
            AddView(button);

            text = new TextView(context);
            text.SetTextColor(Color.Rgb(22,41, 69));
            text.Gravity = Android.Views.GravityFlags.CenterVertical;
            text.TextSize = 11.0f;
            AddView(text);
		}

        public override void LayoutSubviews()
        {
            var padding = (int)(3 * Density);
            var imagePadding = Frame.H / 4;

            int x = 0;
            int y = imagePadding;
            int h = Frame.H - 2 * imagePadding;
            int w = h / 2;

            button.SetFrame(x, y, w, h);

            x = w + imagePadding;
            y = 0;
            w = Frame.W - (x + padding);
            h = Frame.H;

            text.SetFrame(x, y, w, h);
        }
	}
}
