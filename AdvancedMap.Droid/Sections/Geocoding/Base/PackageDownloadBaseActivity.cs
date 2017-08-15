
using System;
using Shared.Droid;
using Shared;
using Android.App;

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
        }

        void OnPackageListUpdated(object sender, EventArgs e)
        {

        }

        void OnPackageListFailed(object sender, EventArgs e)
        {

        }

        void OnPackageStatusChanged(object sender, PackageStatusEventArgs e)
        {

        }

        void OnPackageUpdated(object sender, PackageEventArgs e)
        {

        }

        void OnPackageFailed(object sender, PackageFailedEventArgs e)
        {

        }
    }
}
