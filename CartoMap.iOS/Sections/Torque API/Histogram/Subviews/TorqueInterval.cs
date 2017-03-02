
using System;
using CoreGraphics;
using UIKit;

namespace CartoMap.iOS
{
	public class TorqueInterval : UIView
	{
		public int ElementCount { get; private set; }

		public TorqueInterval()
		{
			BackgroundColor = TorqueHistogram.IntervalColor;
		}

		public void Update(nfloat parentHeight, int elementCount, int maxElements)
		{
			if (elementCount == 0)
			{
				UpdateLayout(0, 0);
				return;
			}

			if (ElementCount == elementCount)
			{
				return;
			}

			ElementCount = elementCount;

			int percent = (elementCount * 100) / maxElements;
			nfloat height = (parentHeight * percent) / 100;
			nfloat margin = parentHeight - height;

			UpdateLayout(height, margin);
		}

		public void Update(nfloat parentHeight, int maxElements)
		{
			int percent = (ElementCount * 100) / maxElements;
			nfloat height = (parentHeight * percent) / 100;
			nfloat margin = parentHeight - height;

			UpdateLayout(height, margin);
		}

		void UpdateLayout(nfloat height, nfloat margin)
		{
			Frame = new CGRect(Frame.X, margin, Frame.Width, height);
		}
	}
}
