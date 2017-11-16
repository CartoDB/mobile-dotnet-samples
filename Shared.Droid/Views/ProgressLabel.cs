
using System;
using System.Timers;
using Android.Animation;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace Shared.Droid
{
    public class ProgressLabel : BaseView
    {
        TextView label;
        BaseView progressBar;

        public bool IsVisible
        {
            get { return Alpha >= 1.0f; }
        }

        public ProgressLabel(Context context) : base(context)
        {
            SetBackgroundColor(Colors.TransparentGray);

            label = new TextView(context);
            label.Gravity = Android.Views.GravityFlags.Center;
            label.SetTextColor(Color.White);
            AddView(label);

            progressBar = new BaseView(context);
            progressBar.SetBackgroundColor(Colors.AppleBlue);
            AddView(progressBar);

            Alpha = 0.0f;
        }

        public override void LayoutSubviews()
        {
            label.SetFrame(0, 0, Frame.W, Frame.H);
        }

        public void Update(string text, float progress)
        {
            Update(text);
            UpdateProgress(progress);
        }

        public void Update(string text)
        {
            if (!IsVisible)
            {
                Show();
            }

            label.Text = text.ToUpper();
        }

        public void Complete(string text)
        {
            Update(text);

            var timer = new Timer();
            timer.Interval = 500;
            timer.Start();

            timer.Elapsed += delegate {

                (Context as BaseActivity).RunOnUiThread(delegate
                {
                    Hide();
                });

                timer.Stop();
                timer.Dispose();
                timer = null;
            };
        }

        public void UpdateProgress(float progress)
        {
            var width = (int)(Frame.W * progress / 100);
            var height = (int)(3 * Density);
            var y = Frame.H - height;

            progressBar.SetFrame(0, y, width, height);
        }

        public void Show()
        {
            AnimateAlpha(1.0f);
        }

        public void Hide()
        {
            AnimateAlpha(0.0f);    
        }

	}
}
