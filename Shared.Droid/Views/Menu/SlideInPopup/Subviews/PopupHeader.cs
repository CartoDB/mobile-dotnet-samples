
using System;
using Android.Content;
using Android.Widget;

namespace Shared.Droid
{
    public class PopupHeader : BaseView
    {
        public int TotalHeight
        {
            get { return (int)(Density * 40); }
        }

        public PopupBackButton BackButton { get; private set; }
        TextView label;
        public PopupCloseButton CloseButton { get; private set; }

        public string Text
        {
            get { return label.Text; }
            set { label.Text = value; LayoutSubviews(); }
        }

        public PopupHeader(Context context, int backIcon, int closeIcon) : base(context)
        {
			label = new TextView(context);
			AddView(label);

            BackButton = new PopupBackButton(context, backIcon);
            AddView(BackButton);

            CloseButton = new PopupCloseButton(context, closeIcon);
            AddView(CloseButton);
        }

        public override void LayoutSubviews()
        {
            var padding = (int)(10 * Density);

            label.Measure(0, 0);

            int x = padding;
            int y = 0;
            int w = label.MeasuredWidth;
            int h = Frame.H;

            label.SetFrame(x, y, w, h);
            BackButton.Frame = new CGRect(x, y, w, h);

            w = h;
            x = Frame.W - w;

            CloseButton.Frame = new CGRect(x, y, w, h);
        }
    }
}
