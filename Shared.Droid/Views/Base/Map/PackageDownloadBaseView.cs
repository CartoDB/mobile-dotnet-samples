
using System;
using System.Collections.Generic;
using Android.Content;
using Carto.PackageManager;

namespace Shared.Droid
{
    public class PackageDownloadBaseView : DownloadBaseView
    {
        public ActionButton Packagebutton { get; private set; }

        public PackageDownloadBaseView(Context context, int infoIcon, int backIcon, int closeIcon, int globalIcon, int wifiOnIcon, int wifiOffIcon) 
            : base(context, infoIcon, backIcon, closeIcon, wifiOnIcon, wifiOffIcon)
        {
            Packagebutton = new ActionButton(context, globalIcon);
            AddButton(Packagebutton);
        }

        public void ShowPackagePopup(List<Package> packages)
        {
            Folder = "";
            Popup.Show();
        }

        public string Folder { get; private set; } = "";

        public void UpdatePackages(List<Package> packages)
        {
            
        }

        public void OnStatusChanged(string id, PackageStatus status)
        {
            
        }

        public void OnDownloadComplete(string id)
        {
            
        }
    }
}
