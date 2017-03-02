using System;
using Android.Content;
using Android.Widget;

namespace CartoMap.Droid
{
	public class TorqueIndicator : RelativeLayout
	{
		public TorqueIndicator(Context context) : base(context)
		{
			SetBackgroundColor(TorqueHistogram.IndicatorColor);
		}

		public void Update(int frameNumber)
		{
			int width = LayoutParameters.Width;
			(LayoutParameters as RelativeLayout.LayoutParams).LeftMargin = width * frameNumber;
		}
	}
}
