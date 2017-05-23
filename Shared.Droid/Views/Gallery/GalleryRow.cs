

using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace Shared.Droid
{
	public class GalleryRow : RelativeLayout
	{
		public override Android.Views.ViewGroup.LayoutParams LayoutParameters
		{
			get { return base.LayoutParameters; }
			set
			{
				base.LayoutParameters = value;
				LayoutSubviews();
			}
		}

		TextView label;
		ImageView image;

		public Type Activity { get; private set; }

		public GalleryRow(Context context, MapGallerySource source) : base(context)
		{
			Background = new ColorDrawable(Colors.CartoRed);

			Activity = source.Type;

			label = new TextView(context);
			label.Text = source.Title.ToUpper();
			label.SetTextColor(Color.White);
			label.Gravity = Android.Views.GravityFlags.Center;

			AddView(label);

			image = new ImageView(context);
			image.SetImageResource(source.ImageResource);
			image.SetScaleType(ImageView.ScaleType.CenterCrop);

			AddView(image);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Elevation = 5;
			}
		}

		public void LayoutSubviews()
		{
			int width = LayoutParameters.Width;
			int height = LayoutParameters.Height;

			int padding = width / 30;

			int x = padding;
			int y = padding;
			int w = width - 2 * padding;
			int h = (height - 3 * padding) / 4 * 3;

			var parameters = new RelativeLayout.LayoutParams(w, h);
			parameters.LeftMargin = x;
			parameters.TopMargin = y;

			image.LayoutParameters = parameters;

			y += h + padding;
			h = (height - 3 * padding) / 4;

			parameters = new RelativeLayout.LayoutParams(w, h);
			parameters.LeftMargin = x;
			parameters.TopMargin = y;

			label.LayoutParameters = parameters;
		}


		public bool Contains(int x, int y)
		{
			Rect rect = new Rect();
			GetHitRect(rect);

			return rect.Contains(x, y);
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
