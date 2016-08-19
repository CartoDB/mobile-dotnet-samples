
using System;
using Carto.PackageManager;

namespace Shared
{
	/**
	 * Listener for package manager events.
	 */
	public class RoutingPackageListener : PackageManagerListener
	{
		PackageManager Manager { get; set; }
		string[] DownloadablePackages { get; set; }

		public EventHandler<EventArgs> OfflinePackageReady;
		public EventHandler<PackageUpdateEventArgs> PackageUpdated;

		public RoutingPackageListener(PackageManager manager, string[] downloadablePackages)
		{
			Manager = manager;
			DownloadablePackages = downloadablePackages;
		}

		public override void OnPackageListUpdated()
		{
			Console.WriteLine("RoutingPackageListener: Package list updated");

			var downloadedPackages = 0;
			var totalPackageCount = DownloadablePackages.Length;

			for (int i = 0; i < totalPackageCount; i++)
			{
				var isDownloaded = GetPackageIfDoesNotExists(DownloadablePackages[i]);

				if (isDownloaded)
				{
					downloadedPackages++;
				}
			}

			// If all downloaded, can start with offline routing
			if (downloadedPackages == totalPackageCount)
			{
				if (OfflinePackageReady != null) {
					OfflinePackageReady(new object(), EventArgs.Empty);
				}
			}
		}

		public override void OnPackageListFailed()
		{
			Console.WriteLine("RoutingPackageListener: Package list update failed");
		}

		public override void OnPackageStatusChanged(string id, int version, PackageStatus status)
		{
		}

		public override void OnPackageCancelled(string id, int version)
		{
		}

		public override void OnPackageUpdated(string id, int version)
		{
			Console.WriteLine("RoutingPackageListener: Offline package updated: " + id);

			if (PackageUpdated != null) 
			{
				PackageUpdated(new object(), new PackageUpdateEventArgs { 
					Id = id, 
					Version = version, 
					IsLastDownloaded = id == DownloadablePackages[DownloadablePackages.Length - 1] 
				});
			}
		}

		public override void OnPackageFailed(string id, int version, PackageErrorType errorType)
		{
			Console.WriteLine("RoutingPackageListener: Offline package update failed: " + id);
		}

		bool GetPackageIfDoesNotExists(string packageId)
		{
			PackageStatus status = Manager.GetLocalPackageStatus(packageId, -1);

			if (status == null)
			{
				Manager.StartPackageDownload(packageId);
				return false;
			}
			else if (status.CurrentAction == PackageAction.PackageActionReady)
			{
				Console.WriteLine(packageId + " is downloaded and ready");
				return true;
			}

			return false;
		}

	}

	public class PackageUpdateEventArgs : EventArgs
	{
		public string Id { get; set; }

		public int Version { get; set; }

		public bool IsLastDownloaded { get; set; }
	}
}

