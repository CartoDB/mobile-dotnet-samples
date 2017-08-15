
using System;
using Shared.Droid;
using Shared;
using Android.App;
using System.Collections.Generic;

namespace AdvancedMap.Droid
{
    public class PackageDownloadBaseActivity : Activity
    {
        protected PackageDownloadBaseView ContentView { get; set; }

        protected BasePackageManagerClient Client { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Client.AttachListener();

			Client.Listener.OnPackageListUpdate += OnPackageListUpdated;
			Client.Listener.OnPackageListFail += OnPackageListFailed;

			Client.Listener.OnPackageStatusChange += OnPackageStatusChanged;

			Client.Listener.OnPackageUpdate += OnPackageUpdated;
			Client.Listener.OnPackageFail += OnPackageFailed;

            ContentView.OnlineSwitch.Clicked += OnSwitchChanged;

            ContentView.Packagebutton.Click += OnPackageButtonClicked;
        }

        protected override void OnPause()
        {
            base.OnPause();

            Client.RemoveListener();

            Client.Listener.OnPackageListUpdate -= OnPackageListUpdated;
            Client.Listener.OnPackageListFail -= OnPackageListFailed;

            Client.Listener.OnPackageStatusChange -= OnPackageStatusChanged;

            Client.Listener.OnPackageUpdate -= OnPackageUpdated;
            Client.Listener.OnPackageFail -= OnPackageFailed;

            ContentView.OnlineSwitch.Clicked += OnSwitchChanged;

            ContentView.Packagebutton.Click -= OnPackageButtonClicked;
        }

        void OnSwitchChanged(object sender, EventArgs e)
        {
            if (ContentView.OnlineSwitch.IsOn)
            {
                SetOnlineMode();
            }
            else
            {
                SetOfflineMode();
            }
        }

        protected virtual void SetOnlineMode()
        {
            
        }

        protected virtual void SetOfflineMode()
        {
            
        }

        void OnPackageButtonClicked(object sender, EventArgs e)
        {
            ContentView.ShowPackagePopup(Client.GetPackages(""));    
        }

        void OnPackageListUpdated(object sender, EventArgs e)
        {
            List<Package> packages = Client.GetPackages(ContentView.Folder);
			RunOnUiThread(delegate
			{
                ContentView.UpdatePackages(packages);
			});
        }

        void OnPackageListFailed(object sender, EventArgs e)
        {
            // TODO Alert
        }

        void OnPackageStatusChanged(object sender, PackageStatusEventArgs e)
        {
			RunOnUiThread(delegate
			{
                ContentView.OnStatusChanged(e.Id, e.Status);
			});
        }

        void OnPackageUpdated(object sender, PackageEventArgs e)
        {
            RunOnUiThread(delegate {
                ContentView.OnDownloadComplete(e.Id);    
            });

        }

        void OnPackageFailed(object sender, PackageFailedEventArgs e)
        {
			// TODO Alert
		}
    }
}
