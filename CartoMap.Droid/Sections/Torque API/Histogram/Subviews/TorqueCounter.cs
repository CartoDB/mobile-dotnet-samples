using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace CartoMap.Droid
{
	public class TorqueCounter : TextView
	{
		public TorqueCounter(Context context) : base(context)
		{
			Gravity = Android.Views.GravityFlags.Center;

			Typeface = Typeface.Create("sans-serif-thin", TypefaceStyle.Bold);
			TextSize = 17f;
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

		List<string> timestamps;
		const int incrementBy = 15;

		public void Update(int frameNumber, int frameCount)
		{
			if (timestamps == null)
			{
				timestamps = new List<string>();

				var date = new DateTime(2016, 9, 15, 12, 14, 0);

				for (int i = 0; i < 256; i++)
				{
					string timestamp = date.ToString("HH:mm dd/MM/yyyy");
					timestamps.Add("  " + timestamp + "  ");
					date = date.AddMinutes(incrementBy);
				}
			}

			Text = timestamps[frameNumber];
			return;

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
