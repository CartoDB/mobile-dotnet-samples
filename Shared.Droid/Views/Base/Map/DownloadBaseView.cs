
using System;
using Android.Content;
using Carto.Layers;
using Carto.PackageManager;

namespace Shared.Droid
{
    public class DownloadBaseView : MapBaseView
    {
        public ProgressLabel ProgressLabel { get; private set; }

        public SwitchButton OnlineSwitch { get; private set; }

        public DownloadBaseView(Context context, 
                                int infoIcon, int backIcon, int closeIcon, 
                                int wifiOnIcon, int wifiOffIcon, int bannerIcon) 
            : base(context, infoIcon, backIcon, closeIcon, bannerIcon)
        {
            ProgressLabel = new ProgressLabel(context);
            AddView(ProgressLabel);

            OnlineSwitch = new SwitchButton(context, wifiOnIcon, wifiOffIcon);
            AddButton(OnlineSwitch);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var height = BottomLabelHeight;
            // I have no idea why it's necessary to substract the additional 7.1 * Density,
            // but otherwise the blue progress bar is hidden
            ProgressLabel.Frame = new CGRect(0, Frame.H - height - ((int)(7.1 * Density)), Frame.W, height);
        }

        public void SetFrame()
        {
            Frame = new CGRect(0, 0, Metrics.WidthPixels, UsableHeight);
        }
    }
}
