
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

        public DownloadBaseView(Context context, int infoIcon, int backIcon, int closeIcon, int wifiOnIcon, int wifiOffIcon) 
            : base(context, infoIcon, backIcon, closeIcon)
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
            ProgressLabel.Frame = new CGRect(0, Frame.H - height, Frame.W, height);
        }

    }
}
