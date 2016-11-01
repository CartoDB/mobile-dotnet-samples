
using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class OptionLabel : TextView
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public bool IsActive { get { return BackgroundColor != Color.White; } }

		Color backgroundColor;
		public Color BackgroundColor 
		{ 
			get { return backgroundColor; } 
			set {
				backgroundColor = value;
				SetBackgroundColor(backgroundColor);
			} 
		}

		public Rect HitRect
		{
			get
			{
				var rect = new Rect();
				GetHitRect(rect);

				return rect;
			}
		}

		public Rect GetGlobalRect(int headerHeight, Rect outerRect)
		{
			Rect rect = HitRect;

			int left = outerRect.Left + rect.Left;
			int top = outerRect.Top + headerHeight + rect.Top;
			int right = outerRect.Left + rect.Right;
			int bottom = outerRect.Top + headerHeight + rect.Bottom;

			return new Rect(left, top, right, bottom);
		}

		public OptionLabel(Context context, NameValuePair option) : base(context)
		{
			Name = option.Name;
			Value = option.Value;

			Text = option.Name.ToUpper();

			Gravity = Android.Views.GravityFlags.Center;

			Typeface = Typeface.Create("Helvetica-Neue", TypefaceStyle.Normal);

			Normalize();
		}

		public void Highlight()
		{
			BackgroundColor = Colors.ActiveMenuItem;
			SetTextColor(Color.White);
		}

		public void Normalize()
		{
			BackgroundColor = Color.White;
			SetTextColor(Color.Rgb(50, 50, 50));
		}

		public void SetLayout(float weight)
		{
			int size = Android.Views.ViewGroup.LayoutParams.WrapContent;

			var parameters = new LinearLayout.LayoutParams(size, size, weight);;
			SetPadding(0, 20, 0, 20);
			LayoutParameters = parameters;
		}

		public void SetRelativeLayout(int width, int height, int total, int counter, int y)
		{
			int x = 0;
			int w = 0;
			int h = height;

			if (total == 2)
			{
				w = width / 2;
			}
			else if (total >= 3)
			{
				w = width / 3;
			}
			else
			{
				w = width;
			}

			if (counter % 3 == 0)
			{
				x = width / 3 * 2;
			}
			else if (counter % 2 == 0)
			{
				if (total == 2)
				{
					x = width / 2;
				}
				else {
					x = width / 3;
				}
			}
			else
			{
				x = 0;
			}

			var parameters = new RelativeLayout.LayoutParams(w, h);
			parameters.LeftMargin = x;
			parameters.TopMargin = y;

			LayoutParameters = parameters;
		}
	}

	public class OptionEventArgs : EventArgs
	{
		public Section Section { get; set; }

		public OptionLabel Option { get; set; }
	}
}

