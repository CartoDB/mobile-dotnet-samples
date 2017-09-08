using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Shared.Droid
{
    public class Banner : BaseView
    {
        ImageView imageView;
        TextView textView;
        BaseView progressBar;

        public string Text
        { 
            get { return textView.Text; } 
            set { textView.Text = value; }
        }

        public Banner(Context context, int imageResource) : base(context)
        {
            SetBackgroundColor(Colors.TransparentGray);

            imageView = new ImageView(context);
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            imageView.SetImageResource(imageResource);
            AddView(imageView);

            textView = new TextView(context);
            textView.SetTextColor(Color.White);
            textView.SetTextSize(Android.Util.ComplexUnitType.Sp, 12);
            textView.Gravity = Android.Views.GravityFlags.Center;
            AddView(textView);

            progressBar = new BaseView(context);
            progressBar.SetBackgroundColor(Colors.AppleBlue);
            AddView(progressBar);
        }

        public override void LayoutSubviews()
        {
            int padding = Frame.H / 4;
            int imageSize = Frame.H - 2 * padding;

            imageView.SetFrame(padding, Frame.H / 2 - imageSize / 2, imageSize, imageSize);
            textView.SetFrame(0, 0, Frame.W, Frame.H);
        }

        internal void UpdateProgress(float progress)
        {
            progressBar.Visibility = ViewStates.Visible;

            var width = (int)((Frame.W * progress) / 100);
            var height = (int)(3 * Density);
            var y = Frame.H - height;

            progressBar.Frame = new CGRect(0, y, width, height);
        }

        internal void HideProgress()
        {
            progressBar.Visibility = ViewStates.Gone;
        }
    }
}
