using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace CartoMap.Droid
{
	public class TorqueInterval : RelativeLayout
	{
		int ElementCount;

		public TorqueInterval(Context context, int width, int margin, int height) : base(context)
		{
			SetBackgroundColor(Color.White);

			var parameters = new LinearLayout.LayoutParams(width, height);
			parameters.SetMargins(0, 0, margin, 0);

			LayoutParameters = parameters;
		}

		public void Update(int parentHeight, int elementCount, int maxElements)
		{
			if (elementCount == 0)
			{
				LayoutParameters.Height = 0;
				return;
			}

			if (ElementCount == elementCount)
			{
				return;
			}

			ElementCount = elementCount;

			int percent = (elementCount * 100) / maxElements;
			int height = (parentHeight * percent) / 100;
			int margin = parentHeight - height;

			var parameters = new LinearLayout.LayoutParams(LayoutParameters.Width, height);
			parameters.SetMargins(0, margin, parameters.RightMargin, 0);

			LayoutParameters = parameters;
		}

		public void Update(int parentHeight, int maxElements)
		{
			int percent = (ElementCount * 100) / maxElements;
			int height = (parentHeight * percent) / 100;
			int margin = parentHeight - height;

			var parameters = new LinearLayout.LayoutParams(LayoutParameters.Width, height);
			parameters.SetMargins(0, margin, parameters.RightMargin, 0);

			LayoutParameters = parameters;
		}

	}
}
