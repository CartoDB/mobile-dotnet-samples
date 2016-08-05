using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Carto.Core;
using Carto.DataSources;
using Carto.PackageManager;
using Carto.Ui;
using Java.IO;

namespace CartoMobileSample
{
	[Activity]
	[ActivityDescription(Description =
						 "A sample demonstrating how to use offline package manager of the Carto Mobile SDK. " +
	                     "The sample downloads the latest package list from Carto online service, " +
	                     "displays this list and allows user to manage offline packages")]
	public class PackageManagerActivity : ListActivity
	{
		const string License = "XTUN3Q0ZBd2NtcmFxbUJtT1h4QnlIZ2F2ZXR0Mi9TY2JBaFJoZDNtTjUvSjJLay9aNUdSVjdnMnJwV" +
			"XduQnc9PQoKcHJvZHVjdHM9c2RrLWlvcy0zLiosc2RrLWFuZHJvaWQtMy4qCnBhY2thZ2VOYW1lPWNv" +
			"bS5udXRpdGVxLioKYnVuZGxlSWRlbnRpZmllcj1jb20ubnV0aXRlcS4qCndhdGVybWFyaz1ldmFsdWF0aW9uC" +
			"nVzZXJLZXk9MTVjZDkxMzEwNzJkNmRmNjhiOGE1NGZlZGE1YjA0OTYK";

		public static PackageManagerTileDataSource DataSource;

		CartoPackageManager packageManager;
		ArrayAdapter<Package> packageAdapter;
		List<Package> packageArray = new List<Package>();

		string currentFolder = ""; // Current 'folder' of the package, for example "Asia/"
		string language = "en"; // Language for the package names. Most major languages are supported

		PackageListener PackageUpdateListener = new PackageListener();

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Register license
			MapView.RegisterLicense(License, ApplicationContext);

			// Create package manager
			File packageFolder = new File(ApplicationContext.GetExternalFilesDir(null), "mappackages");

			if (!(packageFolder.Mkdir() || packageFolder.IsDirectory))
			{
				Toast.MakeText(this, "Could not create package folder!", ToastLength.Short).Show();
			}

			try
			{
				packageManager = new CartoPackageManager("nutiteq.osm", packageFolder.AbsolutePath);
			}
			catch (IOException e)
			{
				Toast.MakeText(this, "Exception: " + e, ToastLength.Short).Show();
				Finish();
			}

			packageManager.PackageManagerListener = PackageUpdateListener;

			packageManager.StartPackageListDownload();

			// Initialize ListView
			SetContentView(HelloMap.Resource.Layout.List);

			packageAdapter = new PackageManagerAdapter(this, HelloMap.Resource.Layout.package_item_row, packageArray);
			ListView.Adapter = packageAdapter;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add("Map");

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnMenuItemSelected(int featureId, IMenuItem item)
		{
			// Using static global variable to pass data. Avoid this in your app (memory leaks etc)!
			DataSource = new PackageManagerTileDataSource(packageManager);

			Intent myIntent = new Intent(this, typeof(PackagedMapActivity));
            StartActivity(myIntent);

			return base.OnMenuItemSelected(featureId, item);
		}

		protected override void OnResume()
		{
			base.OnResume();

			// Always Attach handlers OnResume to avoid memory leaks and objects with multple handlers
			PackageUpdateListener.OnPackageListUpdate += UpdatePackages;
			PackageUpdateListener.OnPackageListFail += UpdatePackages;

			PackageUpdateListener.OnPackageCancel += UpdatePackage;
			PackageUpdateListener.OnPackageUpdate += UpdatePackage;
			PackageUpdateListener.OnPackageStatusChange += UpdatePackage;
			PackageUpdateListener.OnPackageFail += UpdatePackage;
		}

		protected override void OnPause()
		{
			base.OnPause();

			// Always detach handlers OnPause to avoid memory leaks and objects with multple handlers
			PackageUpdateListener.OnPackageListUpdate -= UpdatePackages;
			PackageUpdateListener.OnPackageListFail -= UpdatePackages;

			PackageUpdateListener.OnPackageCancel -= UpdatePackage;
			PackageUpdateListener.OnPackageUpdate -= UpdatePackage;
			PackageUpdateListener.OnPackageStatusChange -= UpdatePackage;
			PackageUpdateListener.OnPackageFail -= UpdatePackage;
		}

		protected override void OnStart()
		{
			base.OnStart();
			packageManager.Start();
		}

		protected override void OnStop()
		{
			base.OnStop();
			packageManager.Stop(false);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			packageManager.PackageManagerListener = null;
		}

		public override void OnBackPressed()
		{
			if (currentFolder == "")
			{
				base.OnBackPressed();
			}
			else {
				currentFolder = currentFolder.Substring(0, currentFolder.LastIndexOf('/', currentFolder.Length - 2) + 1);
				UpdatePackages();
			}
		}

		List<Package> GetPackages()
		{
			Dictionary<string, Package> pkgs = new Dictionary<string, Package>();
			PackageInfoVector packageInfoVector = packageManager.ServerPackages;

			for (int i = 0; i < packageInfoVector.Count; i++)
			{
				PackageInfo packageInfo = packageInfoVector[i];

				// Get the list of names for this package. Each package may have multiple names,
				// packages are grouped using '/' as a separator, so the the full name for Sweden
				// is "Europe/Northern Europe/Sweden". We display packages as a tree, so we need
				// to extract only relevant packages belonging to the current folder.
				StringVector packageNames = packageInfo.GetNames(language);

				for (int j = 0; j < packageNames.Count; j++)
				{
					string packageName = packageNames[j];

					if (!packageName.StartsWith(currentFolder))
					{
						continue; // belongs to a different folder, so ignore
					}

					packageName = packageName.Substring(currentFolder.Length);
					int index = packageName.IndexOf('/');
					Package pkg;

					if (index == -1)
					{
						// This is actual package
						PackageStatus packageStatus = packageManager.GetLocalPackageStatus(packageInfo.PackageId, -1);
						pkg = new Package(packageName, packageInfo, packageStatus);
					}
					else {
						// This is package group
						packageName = packageName.Substring(0, index);
						if (pkgs.ContainsKey(packageName))
						{
							continue;
						}
						pkg = new Package(packageName, null, null);
					}
					pkgs.Add(packageName, pkg);
				}
			}

			return new List<Package>(pkgs.Values);
		}

		void UpdatePackages(object sender, EventArgs e)
		{
			UpdatePackages();	
		}

		void UpdatePackages()
		{
			if (packageAdapter == null)
			{
				return;
			}

			RunOnUiThread(delegate
			{
				packageArray.Clear();
				packageArray.AddRange(GetPackages());
				packageAdapter.NotifyDataSetChanged();
			});
		}

		void UpdatePackage(object sender, PackageEventArgs e)
		{
			UpdatePackage(e.Id);
		}

		void UpdatePackage(object sender, PackageStatusEventArgs e)
		{
			UpdatePackage(e.Id);
		}

		void UpdatePackage(object sender, PackageFailedEventArgs e)
		{
			UpdatePackage(e.Id);
		}

		void UpdatePackage(string id)
		{
			if (packageAdapter == null)
			{
				return;
			}

			RunOnUiThread(delegate {
				// Try to find the package that needs to be updated
				for (int i = 0; i < packageArray.Count; i++)
				{
					Package pkg = packageArray[i];

					if (id.Equals(pkg.Id))
					{
						PackageStatus packageStatus = packageManager.GetLocalPackageStatus(id, -1);
						pkg = new Package(pkg.Name, pkg.Info, packageStatus);
						packageArray.Insert(i, pkg);

						// TODO: it would be much better to only refresh the changed row
						packageAdapter.NotifyDataSetChanged();
					}
				}	
			});
		}

		void DisplayToast(string message)
		{
			RunOnUiThread(delegate { Toast.MakeText(this, message, ToastLength.Short).Show(); });
		}

	}

	#region Supporting Classes

	/**
	* Full package info
	*/
	public class Package
	{
		public string Name { get; private set; }
		public string Id { get; private set; }

		public PackageInfo Info { get; private set; }
		public PackageStatus Status { get; private set; }

		public Package(string name, PackageInfo info, PackageStatus status)
		{
			this.Name = name;
			this.Id = (info != null ? info.PackageId : null);
			this.Info = info;
			this.Status = status;
		}
	}

	/**
	* Holder class for packages containing views for each row in list view.
	*/
	public class PackageHolder : Java.Lang.Object
	{
		public TextView NameView { get; set; }
		public TextView StatusView { get; set; }
		public Button ActionButton { get; set; }
	}

	#endregion
}

