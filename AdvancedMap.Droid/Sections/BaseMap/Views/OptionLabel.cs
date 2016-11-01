
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
			BackgroundColor = Colors.ActionBar;
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
	}
}

