using Carto.PackageManager;
using Carto.Utils;
using System;

namespace CartoMobileSample
{
	public class PackageListener : PackageManagerListener
	{
		public EventHandler OnPackageListUpdate;

		public PackageManager PackageManager { get; set; }

		public string DownloadedPackage { get; set; }

		public PackageListener()
		{
			
		}

		public PackageListener(PackageManager manager)
		{
			PackageManager = manager;
		}

		public PackageListener (PackageManager packageManager, string downloadedPackage)
		{
			PackageManager = packageManager;
			DownloadedPackage = downloadedPackage;
		}

		public override void OnPackageListUpdated ()
		{
			// called when package list is downloaded
			// now you can start downloading packages
			Console.WriteLine("!!!!!!!! ONPACKAGELISTUPDATED");

			if (OnPackageListUpdate != null) {
				OnPackageListUpdate(this, EventArgs.Empty);	
			}

			if (DownloadedPackage == null) {
				return;
			}

			// to make sure that package list is updated, full package download is called here
			if (PackageManager.GetLocalPackage(DownloadedPackage) == null)
			{
				PackageManager.StartPackageDownload(DownloadedPackage);
			}
		}

		public override void OnPackageListFailed ()
		{
			Log.Debug ("OnPackageListFailed");
			// Failed to download package list
		}

		public override void OnPackageStatusChanged (string id, int version, PackageStatus status)
		{
			// a portion of package is downloaded. Update your progress bar here.
			// Notice that the view and SDK are in different threads, so data copy id needed
			Log.Debug ("OnPackageStatusChanged " + id + " ver " + version + " progress " + status.Progress);
		}

		public override void OnPackageCancelled (string id, int version)
		{
			// called when you called cancel package download
			Log.Debug ("OnPackageCancelled");
		}

		public override void OnPackageUpdated (string id, int version)
		{
			// called when package is updated
			Log.Debug ("OnPackageUpdated");
		}

		public override void OnPackageFailed (string id, int version, PackageErrorType errorType)
		{
			// Failed to download package " + id + "/" + version
			Log.Debug ("OnPackageFailed: " + errorType);
		}
	}

	// TODO UPCOMING FUNCTIONALITY IN PACKAGEMANAGERACTIVITY:

	//public class PackageListener : PackageManagerListener
	//{

	//	public override void OnPackageListUpdated()
	//	{
	//		updatePackages();
	//	}

	//	public override void OnPackageListFailed()
	//	{
	//		updatePackages();
	//		displayToast("Failed to download package list");
	//	}

	//	public override void OnPackageStatusChanged(String id, int version, PackageStatus status)
	//	{
	//		updatePackage(id);
	//	}

	//	public override void onPackageCancelled(String id, int version)
	//	{
	//		updatePackage(id);
	//	}

	//	public void onPackageUpdated(String id, int version)
	//	{
	//		updatePackage(id);
	//	}

	//	public override void OnPackageFailed(String id, int version, PackageErrorType errorType)
	//	{
	//		updatePackage(id);
	//		displayToast("Failed to download package " + id + "/" + version + ": " + errorType);
	//	}
	//}

}