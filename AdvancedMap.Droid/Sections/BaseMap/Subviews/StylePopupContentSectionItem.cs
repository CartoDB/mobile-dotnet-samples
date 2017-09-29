using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;
using Shared.Droid;

namespace AdvancedMap.Droid.Sections.BaseMap.Views
{
    public class StylePopupContentSectionItem : BaseView
    {
        ImageView imageView;
        public TextView Label { get; private set; }

        int borderWidth;

        public StylePopupContentSectionItem(Context context, string text, int resource) : base(context)
        {
            SetBackgroundColor(Color.White);

            imageView = new ImageView(context);
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
            imageView.SetImageResource(resource);
            AddView(imageView);

            Label = new TextView(context);
            Label.Text = text;
            Label.SetTextColor(Colors.AppleBlue);
            Label.TextSize = 11.0f;
            AddView(Label);

            borderWidth = (int)(2 * Density);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            int padding = (int)(5 * Density);

            int x = borderWidth;
            int y = borderWidth;
            int w = Frame.W - 2 * borderWidth;
            int h = (Frame.H - 2 * borderWidth) / 3 * 2;

            imageView.SetFrame(x, y, w, h);

            Label.Measure(0, 0);

            y += h + padding;
            h = Label.MeasuredHeight;

            Label.SetFrame(x, y, w, h);
        }

        public void Highlight()
        {
            SetBorderColor(borderWidth, Colors.AppleBlue);
        }

        public void Normalize()
        {
            SetBorderColor(0, Colors.AppleBlue);
        }
    }
}
