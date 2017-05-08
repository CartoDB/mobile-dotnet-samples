
using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace CartoMap.iOS
{
	public class HistogramView : UIView
	{
		public nfloat IntervalWidth { get; set; }

		List<TorqueInterval> intervals;

		public int Count { get { return intervals.Count; } }

		public HistogramView()
		{
			intervals = new List<TorqueInterval>();

			BackgroundColor = TorqueHistogram.Color;

			Layer.CornerRadius = 3;
		}

		public void Clear()
		{
			foreach (TorqueInterval interval in intervals)
			{
				interval.RemoveFromSuperview();	
			}

			intervals.Clear();
		}

		public void Initialize(int frameCount)
		{
			IntervalWidth = Frame.Width / frameCount;

			for (int i = 0; i < frameCount; i++)
			{
				var interval = new TorqueInterval();
				interval.Frame = new CGRect(i * IntervalWidth, 0, IntervalWidth, 0);

				AddSubview(interval);
				intervals.Add(interval);
			}
		}

		public void UpdateIntervalWidth()
		{
			IntervalWidth = Frame.Width / intervals.Count;

			for (int i = 0; i < intervals.Count; i++) 
			{
				TorqueInterval interval = intervals[i];
				interval.Frame = new CGRect(i * IntervalWidth, Frame.Height - interval.Frame.Height, IntervalWidth, interval.Frame.Height);
			}
		}

		public void UpdateAll(int maxElements)
		{
			foreach (TorqueInterval interval in intervals)
			{
				interval.Update(Frame.Height, maxElements);
			}
		}

		public void UpdateElement(int frameNumber, int elementCount, int maxElements)
		{
			intervals[frameNumber].Update(Frame.Height, elementCount, maxElements);
		}
	}
}
