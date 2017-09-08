
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

        public DownloadBaseView(Context context, int infoIcon, int backIcon, int closeIcon, int wifiOnIcon, int wifiOffIcon, bool withBaseLayer = true) 
            : base(context, infoIcon, backIcon, closeIcon)
        {
            ProgressLabel = new ProgressLabel(context);
            AddView(ProgressLabel);

            OnlineSwitch = new SwitchButton(context, wifiOnIcon, wifiOffIcon);
            AddButton(OnlineSwitch);

            if (withBaseLayer) 
            {
                SetOnlineMode();   
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var height = BottomLabelHeight;
            ProgressLabel.Frame = new CGRect(0, Frame.H - height, Frame.W, height);
        }

		CartoOnlineVectorTileLayer onlineLayer;
		CartoOfflineVectorTileLayer offlineLayer;

        public CartoPackageManager Manager { get; set; }

		public void SetOnlineMode()
        {
            if (onlineLayer == null)
            {
                onlineLayer = new CartoOnlineVectorTileLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
            }

            if (offlineLayer != null)
            {
                MapView.Layers.Remove(offlineLayer);
            }

            MapView.Layers.Add(onlineLayer);
        }

        public void SetOfflineMode()
        {
            SetOfflineMode(Manager);    
        }

        public void SetOfflineMode(CartoPackageManager manager)
        {
            if (onlineLayer != null)
            {
                MapView.Layers.Remove((onlineLayer));    
            }

            if (offlineLayer == null)
            {
                offlineLayer = new CartoOfflineVectorTileLayer(manager, CartoBaseMapStyle.CartoBasemapStyleVoyager);
                offlineLayer.Preloading = true;
            }

            MapView.Layers.Add(offlineLayer);
        }
    }
}
