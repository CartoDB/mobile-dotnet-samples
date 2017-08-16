
using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
    public static class ViewExtensions
    {
        public static void SetFrame(this View view, int x, int y, int w, int h)
        {
			var parameters = new RelativeLayout.LayoutParams(w, h);
			parameters.LeftMargin = x;
			parameters.TopMargin = y;

			view.LayoutParameters = parameters;    
        }

		const int Lollipop = 21;
		const int JellyBean = 16;

        public static bool IsLollipopOrHigher(this View view)
		{
            return (int)Build.VERSION.SdkInt >= Lollipop;
		}

        public static bool IsJellyBeanOrHigher(this View view)
		{
			return (int)Build.VERSION.SdkInt >= JellyBean;
		}

		public static void SetBackground(this View view, Android.Graphics.Color color)
		{
			var drawable = new GradientDrawable();
			drawable.SetColor(color);

			if (view.IsJellyBeanOrHigher())
			{
				view.Background = drawable;
			}
			else
			{
				view.SetBackgroundColor(color);
			}
		}

		public static void SetCornerRadius(this View view, int radius)
		{
            if (view.Background is GradientDrawable)
            {
                (view.Background as GradientDrawable).SetCornerRadius(radius);
            }
		}

        public static void SetBorder(this View view, int width, Color color)
        {
			if (view.Background is GradientDrawable)
			{
                (view.Background as GradientDrawable).SetStroke(width, color);
			}
        }
	}
}
