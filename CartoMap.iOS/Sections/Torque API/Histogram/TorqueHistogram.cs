
using System;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace CartoMap.iOS
{
	public class TorqueHistogram : UIView
	{
		public static UIColor Color = UIColor.FromRGBA(215, 82, 75, 200);
		public static UIColor ButtonColor = UIColor.FromRGB(215, 82, 75);
		public static UIColor IndicatorColor = UIColor.FromRGB(14, 122, 254);
		public static UIColor IntervalColor = UIColor.White;

		public EventHandler<HistogramEventArgs> Click;

		public nfloat Margin;
		nfloat BarHeight;
		nfloat CounterHeight;
		nfloat ButtonHeight;
		public nfloat TotalHeight;

		public TorqueButton Button { get; private set; }

		public TorqueIndicator Indicator { get; private set; }

		public TorqueCounter Counter { get; private set; }

		HistogramView Histogram;

		public TorqueHistogram()
		{
			Margin = 6;
			BarHeight = 40;
			CounterHeight = 20;
			ButtonHeight = BarHeight;
			TotalHeight = BarHeight + CounterHeight + Margin;

			Counter = new TorqueCounter();
			AddSubview(Counter);

			Histogram = new HistogramView();
			AddSubview(Histogram);

			Button = new TorqueButton();
			AddSubview(Button);

			Indicator = new TorqueIndicator();
			AddSubview(Indicator);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat x = 0;
			nfloat y = 0;
			nfloat w = CounterHeight * 4;
			nfloat h = CounterHeight;

			Counter.Frame = new CGRect(x, y, w, h);

			y = CounterHeight + Margin;
			w = Frame.Width / 3 * 2;
			h = BarHeight;

			Histogram.Frame = new CGRect(x, y, w, h);

			w = ButtonHeight;
			h = ButtonHeight;
			x = Frame.Width - ButtonHeight;
			y = Frame.Height - ButtonHeight;

			Button.Frame = new CGRect(x, y, w, h);
		}

		public void OnOrientationChange()
		{
			Histogram.UpdateIntervalWidth();
			Indicator.Frame = new CGRect(0, CounterHeight + Margin / 2, Histogram.IntervalWidth, BarHeight + Margin);
		}

		public void Initialize(int frameCount)
		{
			Histogram.Clear();
			Histogram.Initialize(frameCount);

			Indicator.Frame = new CGRect(0, CounterHeight + Margin / 2, Histogram.IntervalWidth, BarHeight + Margin);
		}

		public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
		{
			UITouch touch = (UITouch)evt.AllTouches.AnyObject;
			int x = (int)touch.LocationInView(Histogram).X;

			int frameNumber = (int)(x / Histogram.IntervalWidth);

			if (Click != null)
			{
				Click(this, new HistogramEventArgs { FrameNumber = frameNumber });
				Indicator.Update(frameNumber);
			}
		}

		public void UpdateElement(int frameNumber, int elementCount, int maxElements)
		{
			if (Histogram.Count == 0)
			{
				return;
			}

			Histogram.UpdateElement(frameNumber, elementCount, maxElements);
			Indicator.Update(frameNumber);
		}

		public void UpdateAll(int max)
		{
			Histogram.UpdateAll(max);
		}
	}

	public class HistogramEventArgs : EventArgs
	{
		public int FrameNumber { get; set; }
	}
}
