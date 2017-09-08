
using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	public class CityChoiceMenu : BaseMenu
	{
		public EventHandler<EventArgs> CityTapped;

		List<CityLabel> views = new List<CityLabel>();

		List<BoundingBox> items;
		public List<BoundingBox> Items
		{
			get { return items; }
			set
			{
				items = value;

				foreach (BoundingBox item in items)
				{
					CityLabel view = new CityLabel(context);
					view.BoundingBox = item;
					views.Add(view);
					contentContainer.AddView(view);
				}
			}
		}

		Context context;
		LinearLayout contentContainer;

		public CityChoiceMenu(Context context) : base (context)
		{
			this.context = context;

            SetBackgroundColor(Colors.TransparentGray);

			contentContainer = new LinearLayout(context);
			contentContainer.LayoutParameters = new ViewGroup.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			contentContainer.Orientation = Orientation.Vertical;
			contentContainer.SetGravity(GravityFlags.Right);
			AddView(contentContainer);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			int x = (int)e.GetX();
			int y = (int)e.GetY();

			if (e.Action == MotionEventActions.Up)
			{
				bool isInBox = false;

				foreach (CityLabel view in views)
				{
					if (view.HitRect.Contains(x, y))
					{
						isInBox = true;
						if (CityTapped != null)
						{
							CityTapped(view.BoundingBox, EventArgs.Empty);
						}
					}
				}

				if (!isInBox)
				{
					Hide();
				}
			}

			return true;
		}
	}

	public class CityLabel : RelativeLayout
	{
		BoundingBox box;

		TextView text;

		public Rect HitRect
		{
			get
			{
				var rect = new Rect();
				GetHitRect(rect);

				return rect;
			}
		}

		public BoundingBox BoundingBox
		{
			get { return box; }
			set
			{
				box = value;

				text.Text = box.Name.ToUpper();
			}
		}

		public CityLabel(Context context) : base(context)
		{
			text = new TextView(context);
            text.Gravity = GravityFlags.CenterVertical;

            text.SetTextColor(Colors.CartoNavy);
			text.SetBackgroundColor(Color.Rgb(240, 240, 240));

			int width = (int)(context.Resources.DisplayMetrics.WidthPixels * 0.5);
			int padding = width / 12;

            text.SetPadding(padding, padding, 0, padding);

			var textParams = new RelativeLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
			text.LayoutParameters = textParams;

			AddView(text);

			LayoutParameters = new RelativeLayout.LayoutParams(width, ActionBar.LayoutParams.WrapContent);
			(LayoutParameters as RelativeLayout.LayoutParams).MarginStart = width;
			SetBackgroundColor(Color.Red);
		}
	}
}
