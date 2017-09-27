
using System;
using System.Collections.Generic;
using Carto.PackageManager;

namespace Shared.iOS
{
    public class PackageDownloadBaseController : BaseController
    {
        public PackageDownloadBaseView ContentView { get; set; }

        public BasePackageManagerClient Client { get; set; }

		public string GetPackageFolder(string folder)
		{
			return Utils.GetDocumentDirectory(folder);
		}

		public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (Client.Manager == null)
            {
                return;
            }

            Client.Manager.PackageManagerListener = Client.Listener;
			Client.Listener.OnPackageListUpdate += PackageListUpdated;
			Client.Listener.OnPackageUpdate += PackageUpdated;
			Client.Listener.OnPackageStatusChange += PackageStatusChanged;

			Client.Manager.Start();
			Client.Manager.StartPackageListDownload();

			ContentView.PackageContent.Source.CellSelected += OnCellSelected;

            ContentView.OnlineButton.Switched += OnSwitchChanged;

            ContentView.PackageButton.Click += PackageButtonTapped;

			ContentView.Popup.Header.BackButton.Click += OnPopupBackClicked;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

			if (Client.Manager == null)
			{
				return;
			}

            Client.Manager.PackageManagerListener = null;
			Client.Listener.OnPackageListUpdate -= PackageListUpdated;
			Client.Listener.OnPackageUpdate -= PackageUpdated;
			Client.Listener.OnPackageStatusChange -= PackageStatusChanged;

			Client.Manager.Stop(false);

			ContentView.PackageContent.Source.CellSelected -= OnCellSelected;

            ContentView.OnlineButton.Switched -= OnSwitchChanged;

            ContentView.PackageButton.Click -= PackageButtonTapped;

            ContentView.Popup.Header.BackButton.Click -= OnPopupBackClicked;
        }

		void OnPopupBackClicked(object sender, EventArgs e)
		{
			ContentView.Folder = ContentView.Folder.Substring(0, ContentView.Folder.Length - 1);
			var lastSlash = ContentView.Folder.LastIndexOf("/", StringComparison.Ordinal);

			if (lastSlash == -1)
			{
				ContentView.Folder = "";
				ContentView.HideBackButton();
			}
			else
			{
				ContentView.Folder = ContentView.Folder.Substring(0, lastSlash + 1);
			}

			List<Package> packages = Client.GetPackages(ContentView.Folder);
			ContentView.PackageContent.AddPackages(packages);
		}

        void PackageButtonTapped(object sender, EventArgs e)
		{
			ContentView.Popup.Header.SetText("SELECT A PACKAGE");
			ContentView.Popup.SetContent(ContentView.PackageContent);
			ContentView.Popup.Show();
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
                    ContentView.Popup.ShowBackButton();
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
				string text = "DOWNLOADING PACKAGE: " + progress + "%";

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
            List<Package> packages = Client.GetPackages(ContentView.Folder);
			InvokeOnMainThread(delegate
            {
	            ContentView.UpdatePackages(packages);
            });
		}

		public virtual void SetOnlineMode() { }

        public virtual void SetOfflineMode() { }
    }
}
