
using System;
using Shared.Droid;
using Shared;
using Android.App;
using System.Collections.Generic;
using Android.Widget;
using Java.IO;

namespace AdvancedMap.Droid
{
    public class PackageDownloadBaseActivity : Activity
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

        private void OnPopupBackClicked(object sender, EventArgs e)
        {
            ContentView.Folder = ContentView.Folder.Substring(0, ContentView.Folder.Length - 1);
            var lastSlash = ContentView.Folder.LastIndexOf("/");

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
                    string action = package.ActionText;
                    string id = package.Id;

                    if (action.Equals(Package.ACTION_DOWNLOAD))
                    {
                        Client.Manager.StartPackageDownload(id);
                    }
                    else if (action.Equals(Package.ACTION_PAUSE))
                    {
                        Client.Manager.SetPackagePriority(id, -1);
                    }
                    else if (action.Equals(Package.ACTION_RESUME))
                    {
                        Client.Manager.SetPackagePriority(id, 0);
                    }
                    else if (action.Equals(Package.ACTION_CANCEL))
                    {
                        Client.Manager.CancelPackageTasks(id);
                    }
                    else if (action.Equals(Package.ACTION_REMOVE))
                    {
                        Client.Manager.StartPackageRemove(id);
                    }
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

        public void RunOnBackgroundThread(Action action)
        {
            System.Threading.Tasks.Task.Run(action);
        }
    }
}
