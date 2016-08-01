using System;
using Java.IO;
using Carto.Utils;
using Android.App;

namespace CartoMobileSample
{
	[Activity (Label = "Offline Map")]			
	public class OfflineMap: BaseMapActivity
	{
		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Create package manager folder (Platform-specific)
			var packageFolder = new File (GetExternalFilesDir(null), "packages");
			if (!(packageFolder.Mkdirs() || packageFolder.IsDirectory)) {
				Log.Fatal("Could not create package folder!");
			}

			// Copy bundled tile data to file system, so it can be imported by package manager
			string importPackagePath = new File (GetExternalFilesDir (null), "world_ntvt_0_4.mbtiles").AbsolutePath;
			using (var input = Assets.Open ("world_ntvt_0_4.mbtiles")) {
				using (var output = new System.IO.FileStream (importPackagePath, System.IO.FileMode.Create)) {
					input.CopyTo (output);
				}
			}

			// Initialize map
			string downloadArea = "bbox(-0.8164,51.2382,0.6406,51.7401)"; // London (about 30MB)
			string downloadId = "EE"; // one of ID-s from https://developer.nutiteq.com/guides/packages

			// decice what to download offline
			var toBeDownloaded = downloadId;
			MapSetup.InitializePackageManager (packageFolder.AbsolutePath, importPackagePath, MapView, toBeDownloaded);
		}
	}
}

