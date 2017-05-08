
using System;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using System.Collections.Generic;
using Android.Graphics.Drawables;
using Android.Views;

namespace CartoMap.Droid
{
	public class TorqueHistogram : RelativeLayout
	{
		public static Color BackgroundColor = Color.Argb(200, 215, 82, 75);
		public static Color ButtonColor = Color.Rgb(215, 82, 75);
		public static Color IndicatorColor = Color.Rgb(14, 122, 254);

		public new EventHandler<HistogramEventArgs> Click;

		LinearLayout container;
		RelativeLayout outerContainer;

		TorqueIndicator indicator;

		List<TorqueInterval> intervals;

		int BarHeight { get { return (int)(60 * Context.Resources.DisplayMetrics.Density); } }
		int CounterHeight { get { return (int)(22 * Context.Resources.DisplayMetrics.Density); } }

		int TotalHeight{ get { return BarHeight + CounterHeight; } }

		public TorqueCounter Counter { get; private set; }

		public TorqueButton Button { get; private set; }

		int GetIntervalWidth()
		{
			// TODO test on different devices. Old interval width was hardcoded 5
			return (int)(2f * Context.Resources.DisplayMetrics.Density);
		}

		public TorqueHistogram(Context context) : base(context)
		{
			intervals = new List<TorqueInterval>();

			outerContainer = new RelativeLayout(context);
			AddView(outerContainer);

			container = new LinearLayout(context);
			container.Orientation = Orientation.Horizontal;

			var drawable = new GradientDrawable();
			drawable.SetColor(BackgroundColor);
			drawable.SetCornerRadius(5);
			container.Background = drawable;

			container.LayoutParameters = LayoutUtils.LinearMatchParent();

			indicator = new TorqueIndicator(context);

			Counter = new TorqueCounter(context);

			Button = new TorqueButton(context);
		}

		internal void Initialize(int frameCount, int margin)
		{
			outerContainer.AddView(container);

			AddView(indicator);
			AddView(Counter);
			AddView(Button);

			int intervalMargin = 0;
			int intervalHeight = 0;
			int intervalWidth = GetIntervalWidth();

			int width = frameCount * intervalWidth + frameCount * intervalMargin;
			var containerParams = new RelativeLayout.LayoutParams(width, RelativeLayout.LayoutParams.MatchParent);
			containerParams.SetMargins(0, CounterHeight + margin, 0, margin);
			outerContainer.LayoutParameters = containerParams;

			for (int i = 0; i < frameCount; i++)
			{
				var bar = new TorqueInterval(Context, intervalWidth, intervalMargin, intervalHeight);
				container.AddView(bar);
				intervals.Add(bar);
			}

			var parameters = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.MatchParent, TotalHeight);
			parameters.AddRule(LayoutRules.AlignParentBottom);
			parameters.SetMargins(margin, 0, 0, 0);
			LayoutParameters = parameters;

			var counterParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.WrapContent, CounterHeight);
			counterParams.AddRule(LayoutRules.AlignParentTop);
			Counter.LayoutParameters = counterParams;

			var indictorParams = LayoutUtils.Relative(0, CounterHeight, GetIntervalWidth(), RelativeLayout.LayoutParams.MatchParent);
			indicator.LayoutParameters = indictorParams;

			int buttonSize = BarHeight - 2 * margin;

			var buttonParams = LayoutUtils.Relative(0, 0, buttonSize, buttonSize);
			buttonParams.AddRule(LayoutRules.AlignParentRight);
			buttonParams.AddRule(LayoutRules.AlignParentBottom);
			buttonParams.SetMargins(margin, margin, margin, margin);
			Button.LayoutParameters = buttonParams;
		}

		public void UpdateElement(int frameNumber, int elementCount, int maxElements)
		{
			if (intervals.Count == 0)
			{
				return;
			}

			intervals[frameNumber].Update(TotalHeight, elementCount, maxElements);
			indicator.Update(frameNumber);
		}

		public void UpdateAll(int maxElements)
		{
			foreach (TorqueInterval interval in intervals)
			{
				interval.Update(TotalHeight, maxElements);
			}
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			int x = (int)e.GetX();

			if (e.Action == MotionEventActions.Down)
			{
				int frameNumber = x / GetIntervalWidth();

				if (Click != null)
				{
					Click(this, new HistogramEventArgs { FrameNumber = frameNumber });
					indicator.Update(frameNumber);
				}

				return true;
			}

			return false;
		}
	}

	public class HistogramEventArgs : EventArgs
	{
		public int FrameNumber { get; set; }
	}

	public class LayoutUtils
	{
		public static LinearLayout.LayoutParams LinearMatchParent()
		{
			var matchParent = LinearLayout.LayoutParams.MatchParent;
			return new LinearLayout.LayoutParams(matchParent, matchParent);
		}

		public static RelativeLayout.LayoutParams RelativeMatchParent()
		{
			var matchParent = RelativeLayout.LayoutParams.MatchParent;
			return new RelativeLayout.LayoutParams(matchParent, matchParent);
		}

		public static RelativeLayout.LayoutParams Relative(int x, int y, int width, int height)
		{
			var parameters = new RelativeLayout.LayoutParams(width, height);
			parameters.LeftMargin = x;
			parameters.TopMargin = y;

			return parameters;
		}
	}

}
