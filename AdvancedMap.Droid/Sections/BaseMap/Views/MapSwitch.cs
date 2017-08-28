
using System;
using Android.Widget;
using Android.Content;
using Android.Graphics;
using Shared.Droid;
using Android.Graphics.Drawables;
using Android.Animation;

namespace AdvancedMap.Droid
{
	public class MapSwitch : LinearLayout
	{
		TextView textView;
		CustomSwitch customSwitch;

		public string Text {
			get { return textView.Text; }
			set { textView.Text = value; }
		}

		public EventHandler<EventArgs> Change;

		public bool IsChecked { get; private set; }

		public string ParameterName { get; set; }

		public string ParameterValue
        { 
            get 
            {
                if (ParameterName.Equals("buildings"))
                {
                    // The buildings switch has three options:
                    // 0: no buildings
                    // 1: 2D buildings
                    // 2: 3D buildings
                    return IsChecked ? "2" : "1";    
                }

                return IsChecked ? "1" : "0";
            }
        }

		GradientDrawable background;

		public MapSwitch(Context context) : base(context)
		{
			textView = new TextView(context);
			textView.Gravity = Android.Views.GravityFlags.CenterVertical;
            textView.SetTextColor(Color.White);
			textView.SetPadding(10, 0, 0, 0);
			AddView(textView);
			textView.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, 1);

			customSwitch = new CustomSwitch(context);
			AddView(customSwitch);

			customSwitch.LayoutParameters = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent, 1);

			background = new GradientDrawable();
			background.SetCornerRadius(5);
			background.SetColor(Color.Argb(130, 0, 0, 0).ToArgb());

			Background = background;
		}

		public override bool OnTouchEvent(Android.Views.MotionEvent e)
		{
			if (e.Action == Android.Views.MotionEventActions.Up)
			{
				IsChecked = !IsChecked;

				customSwitch.UpdateBoxPosition(IsChecked);

				if (Change != null)
				{
					Change(this, EventArgs.Empty);
				}
			}


			return true;
		}

		public override string ToString()
		{
			return string.Format("[MapSwitch: Text={0}, IsChecked={1}]", Text, IsChecked);
		}
	}

	public class CustomSwitch : RelativeLayout
	{
		RelativeLayout box;

		public CustomSwitch(Context context) : base(context)
		{
			box = new RelativeLayout(context);

			var background = new GradientDrawable();
			background.SetColor(Colors.ActionBar.ToArgb());
			background.SetCornerRadius(5);

			box.Background = background;

			AddView(box);

			SetBackgroundColor(Color.Gray);
		}

		bool initialized;

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);

			int width = (Parent as Android.Views.View).Width;
			Console.WriteLine(changed);

			if (!initialized)
			{
				int boxWidth = width / 4;

				var parameters = new RelativeLayout.LayoutParams(boxWidth, LayoutParams.MatchParent);
				parameters.LeftMargin = 0;

				box.LayoutParameters = parameters;
				initialized = true;
			}
		}

		public void UpdateBoxPosition(bool isChecked)
		{
			box.Animate().X(isChecked ? box.LayoutParameters.Width : 0);

			int _on = Color.Argb(255, 46, 125, 50).ToArgb();
			int _off = Color.Gray.ToArgb();

			ObjectAnimator animator;

			if (isChecked)
			{
				animator = ObjectAnimator.OfObject(this, "backgroundColor", new ArgbEvaluator(), _off, _on);
			}
			else
			{
				animator = ObjectAnimator.OfObject(this, "backgroundColor", new ArgbEvaluator(), _on, _off);
			}

			animator.SetDuration(200).Start();
		}
	}
}
