
using System;
using System.Collections.Generic;
using Carto.Layers;
using Carto.PackageManager;
using UIKit;

namespace Shared.iOS
{
    public class PackageDownloadBaseView : DownloadBaseView
    {
        public PopupButton PackageButton { get; private set; }

        public PopupSwitchButton OnlineButton { get; private set; }

		public PackagePopupContent PackageContent { get; private set; }

		public PackageDownloadBaseView()
        {
            PackageButton = new PopupButton("icons/icon_global.png");
            AddButton(PackageButton);

            OnlineButton = new PopupSwitchButton("icons/icon_wifi_on.png", "icons/icon_wifi_off.png");
			AddButton(OnlineButton);

            PackageContent = new PackagePopupContent();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        public string Folder { get; set; } = "";

        public void UpdatePackages(List<Package> packages)
        {
            PackageContent.AddPackages(packages);
        }

        public void UpdateFolder(Package package)
        {
            Folder += package.Name + "/";
        }

		CartoOnlineVectorTileLayer onlineLayer;
		CartoOfflineVectorTileLayer offlineLayer;

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

		public void HidePackageDownloadButtons()
		{
            RemoveButton(PackageButton);
            RemoveButton(OnlineButton);
		}

	}
}
