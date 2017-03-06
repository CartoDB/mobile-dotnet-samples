
using System;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace Shared.Droid
{
	public class MenuButton : RelativeLayout
	{
		public new EventHandler<EventArgs> Click;

		int SmallerScreenMetric {
			get {
				int width = context.Resources.DisplayMetrics.WidthPixels;
				int height = Context.Resources.DisplayMetrics.HeightPixels;

				if (width > height) { return height; }
				return width;
			}
		}

		Context context;

		public MenuButton(int resource, Context context) : base(context)
		{
			this.context = context;

			SetBackgroundResource(resource);

			int size = (int)(SmallerScreenMetric / 6.5);
			int margin = size / 5;

			RelativeLayout.LayoutParams parameters = new RelativeLayout.LayoutParams(size, size);
			parameters.RightMargin = margin;
			parameters.BottomMargin = margin;
			parameters.AddRule(LayoutRules.AlignParentRight);
			parameters.AddRule(LayoutRules.AlignParentBottom);

			LayoutParameters = parameters;

			if (Build.VERSION.SdkInt > BuildVersionCodes.Lollipop)
			{
				Elevation = 3;
			}
			else
			{
				// No elevation for you, my friend
			}
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			if (e.Action == MotionEventActions.Down)
			{
				AnimateToScale(1.05f);
			}
			else if (e.Action == MotionEventActions.Up)
			{
				AnimateToScale(1.0f);
				if (Click != null)
				{
					Click(new object(), EventArgs.Empty);
				}
			}
			else if (e.Action == MotionEventActions.Cancel)
			{
				AnimateToScale(1.0f);
			}

			return true;
		}

		void AnimateToScale(float scale)
		{
			Animate().ScaleY(scale).ScaleX(scale).SetDuration(100);
		}
	}
}

