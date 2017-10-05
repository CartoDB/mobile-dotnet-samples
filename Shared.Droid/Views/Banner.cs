﻿﻿
using System;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Shared.Droid;

namespace Shared.Droid
{
    public class Banner : BaseView
    {
        ImageView leftImage;
        TextView label;

        public Banner(Context context, int resource) : base(context)
        {
            SetBackgroundColor(Colors.DarkTransparentGray);

            leftImage = new ImageView(context);
            leftImage.SetScaleType(ImageView.ScaleType.CenterInside);
            leftImage.SetAdjustViewBounds(true);
            leftImage.SetImageResource(resource);
            AddView(leftImage);

            label = new TextView(context);
            label.Gravity = Android.Views.GravityFlags.Center;
            label.SetTextColor(Color.White);
            label.TextSize = 12.0f;
            AddView(label);

            Alpha = 0.0f;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            int padding = (int)(5 * Density);
            int imagePadding = Frame.H / 4;
            int imageSize = Frame.H - 2 * imagePadding;

            int x = imagePadding;
            int y = imagePadding;
            int w = imageSize;
            int h = w;

            leftImage.SetFrame(x, y, w, h);

            x += w + imagePadding;
            w = Frame.W - (2 * w + 4 * imagePadding);
            y = padding;
            h = Frame.H - 2 * padding;

            label.SetFrame(x, y, w, h);
        }

		void Show()
		{
            BringToFront();
            AnimateAlpha(1.0f);
		}

		void Hide()
		{
            AnimateAlpha(0.0f);
		}

		Timer timer;

		public void Show(string text)
		{
			Show();
			label.Text = text;

            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
                timer = null;
            }

			timer = new Timer();
			timer.AutoReset = false;
			timer.Interval = 5000;

			timer.Elapsed += delegate
			{
                (Context as Activity).RunOnUiThread(delegate
				{
					Hide();
				});

				timer.Stop();
				timer = null;
			};

			timer.Start();
		}
    }
}
