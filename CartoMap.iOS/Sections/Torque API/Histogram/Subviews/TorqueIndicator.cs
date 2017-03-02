
using System;
using CoreGraphics;
using UIKit;

namespace CartoMap.iOS
{
	public class TorqueIndicator : UIView
	{
		public TorqueIndicator()
		{
			BackgroundColor = TorqueHistogram.IndicatorColor;
		}

		public void Update(int frameNumber)
		{
			nfloat x = Frame.Width * frameNumber;
			Frame = new CGRect(x, Frame.Y, Frame.Width, Frame.Height);
		}

	}
}
