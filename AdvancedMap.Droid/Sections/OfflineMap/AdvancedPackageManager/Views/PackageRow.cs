
using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class PackageRow : LinearLayout
	{
		LinearLayout textContainer, buttonContainer;

		TextView nameLabel, statusLabel;

		public PMButton Button { get; set; }

		int padding;

		public new string Id { get; private set; }

		public PackageRow(Context context) : base(context)
		{
			SetBackgroundColor(Color.Rgb(240, 240, 240));

			Orientation = Orientation.Horizontal;

			textContainer = new LinearLayout(context);
			textContainer.Orientation = Orientation.Vertical;

			AddView(textContainer);

			buttonContainer = new LinearLayout(context);
			AddView(buttonContainer);

			int height = context.Resources.DisplayMetrics.HeightPixels / 10;
			LayoutParameters = new AbsListView.LayoutParams(LayoutParams.MatchParent, height);

			textContainer.LayoutParameters = new LinearLayout.LayoutParams(0, LayoutParams.MatchParent, 7);
			buttonContainer.LayoutParameters = new LinearLayout.LayoutParams(0, LayoutParams.MatchParent, 3);

			nameLabel = new TextView(context);
			nameLabel.SetTextColor(Color.Black);
			nameLabel.Gravity = Android.Views.GravityFlags.CenterVertical;
			nameLabel.Typeface = Typeface.Create("Helvetica Neue", TypefaceStyle.Normal);
			nameLabel.SetTextSize(Android.Util.ComplexUnitType.Dip, 16);
			textContainer.AddView(nameLabel);

			statusLabel = new TextView(context);
			statusLabel.SetTextColor(Color.DarkGray);
			statusLabel.Gravity = Android.Views.GravityFlags.Top;
			statusLabel.Typeface = Typeface.Create("Helvetica Neue", TypefaceStyle.Normal);

			textContainer.AddView(statusLabel);

			Button = new PMButton(context);
			buttonContainer.AddView(Button);

			buttonContainer.Measure(0, 0);

			padding = buttonContainer.MeasuredHeight / 3;

			Button.LayoutParameters = GetButtonParams(padding);
		}

		public void Update(Package package)
		{
			Id = package.Id;

			nameLabel.Text = package.Name.ToUpper();
			statusLabel.Text = package.GetStatusText();

			ButtonInfo info = package.GetButtonInfo();

			nameLabel.LayoutParameters = GetTextParams(padding, 1);

			if (info.Type == PMButtonType.UpdatePackages)
			{
				statusLabel.LayoutParameters = new LinearLayout.LayoutParams(0, 0, 0);
			}
			else {
				statusLabel.LayoutParameters = GetTextParams(padding, 1);
			}

			Button.Update(info);
		}

		LinearLayout.LayoutParams GetTextParams(int padding, int gravity)
		{
			var parameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, 1);
			parameters.LeftMargin = padding;
			return parameters;
		}

		LinearLayout.LayoutParams GetButtonParams(int padding)
		{
			var parameters = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent, 5);
			parameters.TopMargin = (int)(padding * 1.5);
			parameters.BottomMargin = parameters.TopMargin;
			parameters.MarginEnd = padding;
			parameters.MarginStart = padding;
			return parameters;
		}
	}

	public class PMButton : TextView
	{
		public string PackageId { get; set; }

		public string PackageName { get; set; }

		public int PriorityIndex { get; set; }

		public PMButtonType Type { get; set; }

		public PMButton(Context context) : base(context)
		{
			Gravity = Android.Views.GravityFlags.Center;
			SetTextColor(Color.White);

			var background = new Android.Graphics.Drawables.GradientDrawable();
			background.SetColor(Colors.ActionBar.ToArgb());
			background.SetCornerRadius(5);

			Background = background;
		}

		public void Update(ButtonInfo info)
		{
			Text = info.Text;

			PackageId = info.PackageId;
			PackageName = info.PackageName;
			PriorityIndex = info.PriorityIndex;
			Type = info.Type;

			if (Type == PMButtonType.UpdatePackages)
			{
				Typeface = Typeface.Create("Helvetica Neue", TypefaceStyle.Bold);
				SetTextSize(Android.Util.ComplexUnitType.Dip, 20);
			}
			else {
				Typeface = Typeface.Create("Helvetica Neue", TypefaceStyle.Normal);
				SetTextSize(Android.Util.ComplexUnitType.Dip, 15);
			}
		}

		public override bool OnTouchEvent(Android.Views.MotionEvent e)
		{
			if (e.Action == Android.Views.MotionEventActions.Down)
			{
				Alpha = 0.6f;
			}
			else if (e.Action == Android.Views.MotionEventActions.Up)
			{
				Alpha = 1.0f;
			}
			else if (e.Action == Android.Views.MotionEventActions.Cancel)
			{
				Alpha = 1.0f;
			}

			return base.OnTouchEvent(e);
		}
	}
}
