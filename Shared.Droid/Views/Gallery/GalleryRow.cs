

using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;

namespace Shared.Droid
{
    public class GalleryRow : BaseView
	{
        TextView title, description;
		ImageView image;

		public Type Activity { get; private set; }

		public GalleryRow(Context context, Sample source) : base(context)
		{
            SetBackgroundColor(Color.White);

			Activity = source.Type;

			image = new ImageView(context);
			image.SetImageResource(source.ImageResource);
			image.SetScaleType(ImageView.ScaleType.CenterCrop);
			AddView(image);

			title = new TextView(context);
			title.Text = source.Title.ToUpper();
            title.SetTextColor(Colors.AppleBlue);
            title.Typeface = Typeface.DefaultBold;
            title.TextSize = 14.0f;
            title.Measure(0, 0);
			AddView(title);

            description = new TextView(context);
            description.SetTextColor(Colors.DarkGray);
            description.TextSize = 12.0f;
            description.Text = source.Description;
            AddView(description);

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Elevation = 5;
			}
		}

		public override void LayoutSubviews()
		{
			int padding = 5;
            int imageHeight = Frame.H / 5 * 3;

			int x = padding;
			int y = padding;
            int w = Frame.W - 2 * padding;
			int h = imageHeight;

			image.SetFrame(x, y, w, h);

			y += h + padding;
            h = title.MeasuredHeight;

			title.SetFrame(x, y, w, h);

			y += h + padding;
            h = Frame.H - (imageHeight + h + 2 * padding);

			description.SetFrame(x, y, w, h);
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
