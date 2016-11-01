using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
	public class MapRowView : LinearLayout
	{
		public static int RowId = -1;

		TextView title, description;
		RelativeLayout topBorder;

		public MapRowView(Context context) : base (context) 
		{
			Id = RowId;

			Orientation = Orientation.Vertical;

			title = new TextView(context);

			title.SetTypeface(Typeface.Create("sans-serif-light", TypefaceStyle.Bold), TypefaceStyle.Bold);
			title.SetTextSize(Android.Util.ComplexUnitType.Dip, 16);
			title.SetTextColor(Color.Black);
			title.Gravity = Android.Views.GravityFlags.CenterVertical;

			description = new TextView(context);

			description.SetTypeface(Typeface.Create("sans-serif-thin", TypefaceStyle.Bold), TypefaceStyle.Bold);
			description.SetTextSize(Android.Util.ComplexUnitType.Dip, 13);
			description.SetTextColor(Color.DarkGray);

			topBorder = new RelativeLayout(context);

			AddView(topBorder);
			AddView(title);
			AddView(description);
		}

		public void Update(Type type, int position)
		{
			LayoutParams parameters = new LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent);
			parameters.SetMargins(10, 10, 10, 10);

			title.Text = type.GetTitle();
			title.LayoutParameters = parameters;

			ColorDrawable background;

			if (type.IsHeader())
			{
				background = new ColorDrawable(Color.Black);
				if (position == 0)
				{
					topBorder.LayoutParameters = new LayoutParams(0, 0);
				}
				else {
					parameters = new LayoutParams(ViewGroup.LayoutParams.MatchParent, 15);
					topBorder.LayoutParameters = parameters;
				}

				description.Text = "";
				description.LayoutParameters = new LayoutParams(0, 0);
				SetBackgroundColor(Color.Rgb(200, 200, 200));
			}
			else {
				background = null;
				description.Text = type.GetDescription();
				description.LayoutParameters = parameters;
				SetBackgroundColor(Color.Rgb(240, 240, 240));
			}

			topBorder.Background = background;
		}

	}
}

