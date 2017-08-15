
using System;
using Android.Content;

namespace Shared.Droid
{
    public class DownloadBaseView : MapBaseView
    {
        public ProgressLabel ProgressLabel { get; private set; }

        public DownloadBaseView(Context context, int backIcon, int closeIcon) : base(context, backIcon, closeIcon)
        {
            ProgressLabel = new ProgressLabel(context);
            AddView(ProgressLabel);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var height = BottomLabelHeight;
            ProgressLabel.Frame = new CGRect(0, Frame.H - height, Frame.W, height);
        }
    }
}
