

using System;
using Android.App;
using Android.Content;
using Android.Graphics;
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

		public Activity Activity { get; private set; }

		public GalleryRow(Context context, MapGallerySource source) : base(context)
		{
			label = new TextView(context);
			label.Text = source.Title.ToUpper();
			label.SetTextColor(Color.White);

			AddView(label);

			image = new ImageView(context);
			image.SetBackgroundColor(Color.Blue);
			AddView(image);
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
	}
}
