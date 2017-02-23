using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace CartoMap.Droid
{
	public class TorqueCounter : TextView
	{
		public static Color TransparentGray = Color.Argb(160, 50, 50, 50);

		public TorqueCounter(Context context) : base(context)
		{
			Gravity = Android.Views.GravityFlags.Center;

			int width = 180;
			int height = (int)(width / 2.2f);

			Typeface = Typeface.Create("sans-serif-thin", TypefaceStyle.Bold);
			TextSize = 17f;
			SetTextColor(Color.White);

			GradientDrawable drawable = new GradientDrawable();
			drawable.SetCornerRadius(5);
			drawable.SetColor(TransparentGray);
			Background = drawable;

			var parameters = new RelativeLayout.LayoutParams(width, height);
			int padding = 30;

			parameters.SetMargins(0, padding, padding, 0);
			parameters.AddRule(LayoutRules.AlignParentTop);
			parameters.AddRule(LayoutRules.AlignParentEnd);
			LayoutParameters = parameters;

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Elevation = 10;
			}

		}

		public void Update(int frameNumber, int frameCount)
		{
			string number = "";

			if (frameCount > 100)
			{
				if (frameNumber < 10)
				{
					number = "00" + frameNumber;
				}
				else if (frameNumber < 100)
				{
					number = "0" + frameNumber;
				}
				else
				{
					number = frameNumber.ToString();
				}
			}

			Text = number + "/" + frameCount;
		}
	}
}
