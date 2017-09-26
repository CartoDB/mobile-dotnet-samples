
using System;
using System.Collections.Generic;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS.BaseClasses
{
    public class PackageDownloadBaseController : UIViewController
    {
		// This must be initialized in child classes
		protected PackageDownloadBaseView ContentView { get; set; }

        // This must be initialized in child classes
		protected BasePackageManagerClient Client { get; set; }

		public string GetPackageFolder(string folder)
		{
            return Utils.GetDocumentDirectory(folder);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			Client.AttachListener();

			Client.Listener.OnPackageListUpdate += GeocodingPackageListUpdated;
			Client.Listener.OnPackageUpdate += GeocodingPackageUpdated;
			Client.Listener.OnPackageStatusChange += GeocodingPackageStatusChanged;

			Client.Manager.Start();
			Client.Manager.StartPackageListDownload();

			ContentView.PackageContent.Source.CellSelected += OnCellSelected;

			ContentView.OnlineButton.Switched += OnSwitchChanged;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			Client.Listener.OnPackageListUpdate -= GeocodingPackageListUpdated;
			Client.Listener.OnPackageUpdate -= GeocodingPackageUpdated;
			Client.Listener.OnPackageStatusChange -= GeocodingPackageStatusChanged;

			Client.RemoveListener();

			Client.Manager.Stop(false);

			ContentView.PackageContent.Source.CellSelected -= OnCellSelected;

			ContentView.OnlineButton.Switched -= OnSwitchChanged;
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

		void GeocodingPackageListUpdated(object sender, EventArgs e)
		{
			List<Package> packages = Client.GetPackages(ContentView.Folder);
			InvokeOnMainThread(delegate
			{
				ContentView.UpdatePackages(packages);
			});
		}

		void GeocodingPackageStatusChanged(object sender, PackageStatusEventArgs e)
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
