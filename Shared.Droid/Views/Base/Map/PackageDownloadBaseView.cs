
using System;
using System.Collections.Generic;
using Android.Content;
using Carto.PackageManager;

namespace Shared.Droid
{
    public class PackageDownloadBaseView : DownloadBaseView
    {
        public ActionButton Packagebutton { get; private set; }

        public PackagePopupContent PackageContent { get; private set; }

        public PackageDownloadBaseView(Context context, int infoIcon, int backIcon, int closeIcon, int globalIcon, int wifiOnIcon, int wifiOffIcon, int forwardIcon, bool withBaseLayer = true) 
            : base(context, infoIcon, backIcon, closeIcon, wifiOnIcon, wifiOffIcon, withBaseLayer)
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

        public string Folder { get; set; } = "";

        public void UpdatePackages(List<Package> packages)
        {
            
        }

        public void OnDownloadComplete(string id)
        {
			
		}
    }
}
