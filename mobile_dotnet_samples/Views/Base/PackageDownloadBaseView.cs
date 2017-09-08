
using System;
using System.Collections.Generic;
using UIKit;

namespace Shared.iOS
{
    public class PackageDownloadBaseView : DownloadBaseView
    {
        protected PopupButton PackageButton { get; private set; }

        public PopupSwitchButton OnlineButton { get; private set; }

		public PackagePopupContent PackageContent { get; private set; }

		public PackageDownloadBaseView()
        {
            PackageButton = new PopupButton("icons/icon_global.png");
            AddButton(PackageButton);

            OnlineButton = new PopupSwitchButton("icons/icon_wifi_on.png", "icons/icon_wifi_off.png");
			AddButton(OnlineButton);

            PackageContent = new PackagePopupContent();

            PackageButton.AddGestureRecognizer(new UITapGestureRecognizer(PackageButtonTapped));
        }

        void PackageButtonTapped()
        {
            Popup.Header.SetText("SELECT A PACKAGE TO DOWNLOAD");
            Popup.SetContent(PackageContent);
            Popup.Show();
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
    }
}
