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

        public BaseGeocodingController()
        {
            string baseFolder = Utils.GetDocumentDirectory();
            Geocoding = new Geocoding(baseFolder);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

			Geocoding.Listener.OnPackageListUpdate += GeocodingPackageListUpdated;
			Geocoding.Listener.OnPackageUpdate += GeocodingPackageUpdated;
			Geocoding.Listener.OnPackageStatusChange += GeocodingPackageStatusChanged;

			Geocoding.Manager.Start();
			Geocoding.Manager.StartPackageListDownload();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

			Geocoding.Listener.OnPackageListUpdate -= GeocodingPackageListUpdated;
			Geocoding.Listener.OnPackageUpdate -= GeocodingPackageUpdated;
			Geocoding.Listener.OnPackageStatusChange -= GeocodingPackageStatusChanged;

			Geocoding.Manager.Stop(false);
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
            
		}

		void GeocodingPackageUpdated(object sender, PackageEventArgs e)
		{
            
		}

	}
}
