using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace Shared.Droid
{
	public class MapRowView : RelativeLayout
	{
		public string Title { 
			get { 
				return titleView.Text; 
			} set { 
				titleView.Text = value; 
			} 
		}

		public string Description { 
			get { 
				return descriptionView.Text; 
			} set { 
				descriptionView.Text = value; 
			}
		}

		TextView titleView, descriptionView;

		public MapRowView(Context context) : base (context)
		{
			Color nearWhite = Color.Rgb(240, 240, 240);

			titleView = new TextView(context);
			titleView.SetTypeface(Typeface.Create("sans-serif-light", TypefaceStyle.Bold), TypefaceStyle.Bold);
			titleView.SetTextSize(Android.Util.ComplexUnitType.Dip, 15);
			titleView.SetTextColor(nearWhite);

			descriptionView = new TextView(context);
			descriptionView.SetTypeface(Typeface.Create("sans-serif-thin", TypefaceStyle.Bold), TypefaceStyle.Bold);
			descriptionView.SetTextSize(Android.Util.ComplexUnitType.Dip, 10);
			descriptionView.SetTextColor(nearWhite);

			AddView(titleView);
			AddView(descriptionView);
		}

		public void Update(Type type)
		{
			Title = type.GetTitle();
			Description = type.GetDescription();

			int width = this.LayoutParameters.Width;
			int height = this.LayoutParameters.Height;

			int titleHeight = (int)(height / 3.5);
			int descHeight = height - titleHeight;

			int padding = width / 50;

			RelativeLayout.LayoutParams titleParams = new RelativeLayout.LayoutParams(width - padding, titleHeight);
			titleParams.LeftMargin = padding;

			RelativeLayout.LayoutParams descriptionParams = new RelativeLayout.LayoutParams(width - padding, descHeight);
			descriptionParams.TopMargin = titleHeight;
			descriptionParams.LeftMargin = padding;

			titleView.LayoutParameters = titleParams;
			descriptionView.LayoutParameters = descriptionParams;
		}
	}
}

