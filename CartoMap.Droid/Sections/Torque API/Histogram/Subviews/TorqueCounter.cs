using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using Shared;

namespace CartoMap.Droid
{
	public class TorqueCounter : TextView
	{
		public TorqueCounter(Context context) : base(context)
		{
			Gravity = Android.Views.GravityFlags.Center;

			Typeface = Typeface.Create("sans-serif-thin", TypefaceStyle.Bold);
			TextSize = 30f / context.Resources.DisplayMetrics.Density;
			SetTextColor(Color.White);

			GradientDrawable drawable = new GradientDrawable();
			drawable.SetCornerRadius(5);
			drawable.SetColor(TorqueHistogram.BackgroundColor);
			Background = drawable;

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Elevation = 10;
			}
		}

		// Map data timespan / 256  - timestep for one torque animation frame 
		const int incrementBy = 15;

		public void Update(int frameNumber, int frameCount)
		{
			if (!TorqueUtils.Initialized)
			{
				TorqueUtils.Initialize();
			}

			Text = TorqueUtils.GetText(frameNumber);
		}

		public void Update(int frameNumber)
		{
			if (!Text.Contains("/"))
			{
				return;
			}

			int frameCount = 0;

			bool success = int.TryParse(Text.Split('/')[1], out frameCount);

			if (!success)
			{
				return;
			}

			Update(frameNumber, frameCount);
		}
	}
}
