﻿
using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Shared.iOS;

namespace AdvancedMap.iOS
{
    public class BaseGeocodingController : BaseController
    {
        public Geocoding Geocoding { get; set; }

        // This is initialized in GeocodingController or ReverseGeocodingController 
        // as GeocodingView or ReverseGeocodingView, respectively
        public BaseGeocodingView ContentView { get; set; }

        protected const string ApiKey = "mapzen-e2gmwsC";

		public BaseGeocodingController()
        {
            string baseFolder = Utils.GetDocumentDirectory();
            Geocoding = new Geocoding(baseFolder);
            Geocoding.ApiKey = ApiKey;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Geocoding.AttachListener();

			Geocoding.Listener.OnPackageListUpdate += GeocodingPackageListUpdated;
			Geocoding.Listener.OnPackageUpdate += GeocodingPackageUpdated;
			Geocoding.Listener.OnPackageStatusChange += GeocodingPackageStatusChanged;

			Geocoding.Manager.Start();
			Geocoding.Manager.StartPackageListDownload();

            ContentView.PackageContent.Source.CellSelected += OnCellSelected;

            ContentView.OnlineButton.Switched += OnSwitchChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

			Geocoding.Listener.OnPackageListUpdate -= GeocodingPackageListUpdated;
			Geocoding.Listener.OnPackageUpdate -= GeocodingPackageUpdated;
			Geocoding.Listener.OnPackageStatusChange -= GeocodingPackageStatusChanged;

            Geocoding.RemoveListener();

			Geocoding.Manager.Stop(false);

            ContentView.PackageContent.Source.CellSelected -= OnCellSelected;

            ContentView.OnlineButton.Switched -= OnSwitchChanged;
        }

        void OnCellSelected(object sender, EventArgs e)
        {
            var package = sender as Package;

            if (package.IsGroup)
            {
                ContentView.UpdateFolder(package);

                List<Package> packages = Geocoding.GetPackages(ContentView.Folder);

                InvokeOnMainThread(delegate
                {
                    ContentView.UpdatePackages(packages);
                });
            }
            else 
            {
                if (package.ActionText == Package.ACTION_DOWNLOAD)
                {
                    InvokeOnMainThread(delegate {
                       ContentView.ProgressLabel.Show(); 
                    });
                }

                Geocoding.HandlePackageStatusChange(package);
            }
        }

        void GeocodingPackageListUpdated(object sender, EventArgs e)
		{
			List<Package> packages = Geocoding.GetPackages(ContentView.Folder);
            InvokeOnMainThread(delegate {
               ContentView.UpdatePackages(packages); 
            });
		}

		void GeocodingPackageStatusChanged(object sender, PackageStatusEventArgs e)
		{
			int progress = (int)e.Status.Progress;
			Package current = Geocoding.CurrentDownload;

            InvokeOnMainThread(delegate {

                string text = "DOWNLOAD PACKAGE: " + progress + "%";

                if (current != null)
                {
                    string name = current.Name;
                    text = "DOWNLOADING " + name.ToUpper() + ": " + progress + "%";
                }

                ContentView.ProgressLabel.Update(text, progress);

                if (current != null)
                {
                    string id = current.Id;
                    ContentView.PackageContent.FindAndUpdate(id, e.Status);
                }

            });
		}

		void GeocodingPackageUpdated(object sender, PackageEventArgs e)
		{
            
		}

        void OnSwitchChanged(object sender, EventArgs e)
        {
            if (ContentView.OnlineButton.IsOn)
            {
                SetOnlineMode();
            }
            else
            {
                SetOfflineMode();
            }
        }

		public virtual void SetOnlineMode() { }

		public virtual void SetOfflineMode() { }
	}
}
