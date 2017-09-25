
using System;
using Shared.Droid;
using Shared;
using Android.App;
using System.Collections.Generic;
using Android.Widget;
using Java.IO;

namespace AdvancedMap.Droid
{
    public class PackageDownloadBaseActivity : BaseActivity
    {
        protected PackageDownloadBaseView ContentView { get; set; }

        protected BasePackageManagerClient Client { get; set; }

        public string GetPackageFolder(string folder)
        {
            var directory = new File(GetExternalFilesDir(null), folder);

            if (!directory.Exists())
            {
                directory.Mkdir();
            }

            return directory.AbsolutePath;
        }

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

            Client.Manager.Start();
            Client.Manager.StartPackageListDownload();

            ContentView.OnlineSwitch.Clicked += OnSwitchChanged;
            ContentView.Packagebutton.Clicked += OnPackageButtonClicked;

            ContentView.PackageContent.List.ItemClick += OnListItemClicked;

            ContentView.Popup.Header.BackButton.Click += OnPopupBackClicked;
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

            Client.Manager.Stop(false);

            ContentView.OnlineSwitch.Clicked += OnSwitchChanged;
            ContentView.Packagebutton.Clicked -= OnPackageButtonClicked;

            ContentView.PackageContent.List.ItemClick -= OnListItemClicked;

            ContentView.Popup.Header.BackButton.Click -= OnPopupBackClicked;
        }

        void OnPopupBackClicked(object sender, EventArgs e)
        {
            ContentView.Folder = ContentView.Folder.Substring(0, ContentView.Folder.Length - 1);
            var lastSlash = ContentView.Folder.LastIndexOf("/", StringComparison.Ordinal);

            if (lastSlash == -1)
            {
                ContentView.Folder = "";
                ContentView.Popup.HideBackButton();
            }
            else
            {
                ContentView.Folder = ContentView.Folder.Substring(0, lastSlash + 1); 
            }

            List<Package> packages = Client.GetPackages(ContentView.Folder);
            ContentView.PackageContent.AddPackages(packages);
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
            ContentView.ShowPackagePopup(Client.GetPackages(ContentView.Folder));    
        }


        void OnListItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            var package = ContentView.PackageContent.Adapter.Packages[e.Position];

            if (package.IsGroup)
            {
                ContentView.Folder = ContentView.Folder + package.Name + "/";

                RunOnBackgroundThread(delegate
                {
                    List<Package> packages = Client.GetPackages(ContentView.Folder);
                    RunOnUiThread(delegate
                    {
                        ContentView.PackageContent.AddPackages(packages);
                        ContentView.Popup.ShowBackButton();
                    });
                });

            }
            else
            {
                RunOnBackgroundThread(delegate
                {
                    Client.HandlePackageStatusChange(package);
                });
            }
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
            Package current = Client.CurrentDownload;
            int progress = (int)e.Status.Progress;

			RunOnUiThread(delegate
			{
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

        void OnPackageUpdated(object sender, PackageEventArgs e)
        {
            var packages = Client.GetPackages(ContentView.Folder);

            RunOnUiThread(delegate {
                ContentView.PackageContent.AddPackages(packages);
            });

        }

        void OnPackageFailed(object sender, PackageFailedEventArgs e)
        {
			// TODO Alert
		}

    }
}
