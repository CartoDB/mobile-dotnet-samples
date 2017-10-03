
using System;
using System.Collections.Generic;
using Android.Content;
using Carto.Layers;
using Carto.PackageManager;

namespace Shared.Droid
{
    public class PackageDownloadBaseView : DownloadBaseView
    {
        public ActionButton Packagebutton { get; private set; }

        public PackagePopupContent PackageContent { get; private set; }

        public PackageDownloadBaseView(Context context, 
                                       int infoIcon, int backIcon, int closeIcon, int globalIcon, 
                                       int wifiOnIcon, int wifiOffIcon, int forwardIcon, int bannerIcon) 
            : base(context, infoIcon, backIcon, closeIcon, wifiOnIcon, wifiOffIcon, bannerIcon)
        {
            Packagebutton = new ActionButton(context, globalIcon);
            AddButton(Packagebutton);

            PackageContent = new PackagePopupContent(context, forwardIcon);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            PackageContent.Adapter.Width = Frame.W;
        }

        public void ShowPackagePopup(List<Package> packages)
        {
            PackageContent.AddPackages(packages);
            Popup.SetPopupContent(PackageContent);
            Popup.Header.Text = "SELECT A PACKAGE";
            Popup.Show();
        }

        public void HidePackageDownloadButtons()
        {
            RemoveButton(Packagebutton);
            RemoveButton(OnlineSwitch);
        }

        public string Folder { get; set; } = "";

        public void UpdatePackages(List<Package> packages)
        {
            
        }

        public void OnDownloadComplete(string id)
        {
			
		}

        CartoOnlineVectorTileLayer onlineLayer;
        CartoOfflineVectorTileLayer offlineLayer;

        public void SetOnlineMode()
        {
            System.Threading.Tasks.Task.Run(delegate
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
            });
        }

        public void SetOfflineMode(CartoPackageManager manager)
        {
            System.Threading.Tasks.Task.Run(delegate
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
            });
        }
    }
}
