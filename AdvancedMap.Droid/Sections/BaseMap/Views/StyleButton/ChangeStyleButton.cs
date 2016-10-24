
using System;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace AdvancedMap.Droid
{
	public class ChangeStyleButton : RelativeLayout
	{
		public EventHandler<StyleEventArgs> StyleChanged;

		public ChangeStyleButton(Context context) : base(context)
		{
			DisplayMetrics metrics = context.Resources.DisplayMetrics;
			int size = (int)(metrics.WidthPixels / 6.5);

			GradientDrawable background = new GradientDrawable();
			background.SetCornerRadius(size / 2);
			background.SetColor(Android.Graphics.Color.Yellow.ToArgb());

			Background = background;

			int margin = size / 5;

			RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(size, size);
			parameters.RightMargin = margin;
			parameters.BottomMargin = margin;
			parameters.AddRule(LayoutRules.AlignParentRight);
			parameters.AddRule(LayoutRules.AlignParentBottom);

			LayoutParameters = parameters;

			if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
			{
				Elevation = 10;
			}
			else 
			{
				// No elevation for you, my friend
			}
		}
	}

	public class StyleEventArgs : EventArgs
	{
		
	}
}

