using Carto.PackageManager;
using Carto.Utils;
using System;

namespace CartoMobileSample
{
	public class PackageListener : PackageManagerListener
	{
		public EventHandler<EventArgs> OnPackageListUpdate;
		public EventHandler<EventArgs> OnPackageListFail;

		public EventHandler<PackageEventArgs> OnPackageCancel;
		public EventHandler<PackageEventArgs> OnPackageUpdate;
		public EventHandler<PackageStatusEventArgs> OnPackageStatusChange;
		public EventHandler<PackageFailedEventArgs> OnPackageFail;

		public PackageManager PackageManager { get; set; }

		public string DownloadedPackage { get; set; }

		public PackageListener()
		{
			
		}

		public PackageListener (PackageManager packageManager, string downloadedPackage)
		{
			PackageManager = packageManager;
			DownloadedPackage = downloadedPackage;
		}

		public override void OnPackageListUpdated ()
		{
			Console.WriteLine("PackageListener: OnPackageListUpdated");
			// Called when package list is downloaded.
			// Now you can start downloading packages
			if (OnPackageListUpdate != null) {
				OnPackageListUpdate(this, EventArgs.Empty);	
			}

			if (DownloadedPackage == null) {
				return;
			}

			// To make sure that package list is updated, full package download is called here
			if (PackageManager.GetLocalPackage(DownloadedPackage) == null)
			{
				PackageManager.StartPackageDownload(DownloadedPackage);
			}
		}

		public override void OnPackageListFailed ()
		{
			Console.WriteLine("PackageListener: OnPackageListFailed");
			if (OnPackageListFail != null)
			{
				OnPackageListFail(this, EventArgs.Empty);
			}
		}

		public override void OnPackageStatusChanged (string id, int version, PackageStatus status)
		{
			Console.WriteLine("PackageListener: OnPackageStatusChanged");
			// A portion of package is downloaded. Update your progress bar here.
			// Notice that the view and SDK are in different threads, so data copy id needed
			if (OnPackageStatusChange != null)
			{
				OnPackageStatusChange(this, new PackageStatusEventArgs { Id = id, Version = version, Status = status });
			}
		}

		public override void OnPackageCancelled (string id, int version)
		{
			Console.WriteLine("PackageListener: OnPackageCancelled");
			// Called when you called cancel package download
			if (OnPackageCancel != null)
			{
				OnPackageCancel(this, new PackageEventArgs { Id = id, Version = version });
			}
		}

		public override void OnPackageUpdated (string id, int version)
		{
			Console.WriteLine("PackageListener: OnPackageUpdated");
			// Called when package is updated
			if (OnPackageUpdate != null)
			{
				OnPackageUpdate(this, new PackageEventArgs { Id = id, Version = version });
			}
		}

		public override void OnPackageFailed (string id, int version, PackageErrorType errorType)
		{
			Console.WriteLine("PackageListener: OnPackageFailed");
			// Failed to download package " + id + "/" + version
			if (OnPackageFail != null)
			{
				OnPackageFail(this, new PackageFailedEventArgs { Id = id, Version = version, ErrorType = errorType });
			}
		}
	}

	public class PackageEventArgs
	{
		public string Id { get; set; }

		public int Version { get; set; }
	}

	public class PackageStatusEventArgs : PackageEventArgs
	{
		public PackageStatus Status { get; set; }
	}

	public class PackageFailedEventArgs : PackageEventArgs
	{
		public PackageErrorType ErrorType { get; set; }
	}

}