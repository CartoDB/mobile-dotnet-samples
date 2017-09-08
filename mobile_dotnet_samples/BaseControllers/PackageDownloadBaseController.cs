
using System;
using System.Collections.Generic;
using Carto.PackageManager;

namespace Shared.iOS
{
    public class PackageDownloadBaseController : BaseController
    {
        public PackageDownloadBaseView ContentView { get; set; }

        public BasePackageManagerClient Client { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Client = new BasePackageManagerClient();

            Client.Listener = new PackageListener();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            Client.Manager.PackageManagerListener = Client.Listener;
			Client.Listener.OnPackageListUpdate += PackageListUpdated;
			Client.Listener.OnPackageUpdate += PackageUpdated;
			Client.Listener.OnPackageStatusChange += PackageStatusChanged;

			Client.Manager.Start();
			Client.Manager.StartPackageListDownload();

			ContentView.PackageContent.Source.CellSelected += OnCellSelected;

            ContentView.OnlineButton.Switched += OnSwitchChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            Client.Manager.PackageManagerListener = null;
			Client.Listener.OnPackageListUpdate -= PackageListUpdated;
			Client.Listener.OnPackageUpdate -= PackageUpdated;
			Client.Listener.OnPackageStatusChange -= PackageStatusChanged;

			Client.Manager.Stop(false);

			ContentView.PackageContent.Source.CellSelected -= OnCellSelected;

            ContentView.OnlineButton.Switched -= OnSwitchChanged;
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

		void OnCellSelected(object sender, EventArgs e)
		{
			var package = sender as Package;

			if (package.IsGroup)
			{
				ContentView.UpdateFolder(package);

				List<Package> packages = Client.GetPackages(ContentView.Folder);

				InvokeOnMainThread(delegate
				{
					ContentView.UpdatePackages(packages);
				});
			}
			else
			{
				if (package.ActionText == Package.ACTION_DOWNLOAD)
				{
					InvokeOnMainThread(delegate
					{
						ContentView.ProgressLabel.Show();
					});
				}

				Client.HandlePackageStatusChange(package);
			}
		}

        void PackageListUpdated(object sender, EventArgs e)
		{
			List<Package> packages = Client.GetPackages(ContentView.Folder);
			InvokeOnMainThread(delegate
			{
				ContentView.UpdatePackages(packages);
			});
		}

        void PackageStatusChanged(object sender, PackageStatusEventArgs e)
		{
			int progress = (int)e.Status.Progress;
			Package current = Client.CurrentDownload;

			InvokeOnMainThread(delegate
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

        void PackageUpdated(object sender, PackageEventArgs e)
		{

		}

		public virtual void SetOnlineMode() { }

        public virtual void SetOfflineMode() { }
    }
}
