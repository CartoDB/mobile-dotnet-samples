using System;
using System.Collections.Generic;
using System.Linq;
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
	[Activity(Label = "PackageManager", Icon = "@drawable/icon")]
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

		public List<Package> Packages { 
			get {
				// Extension method. cf CommonMapExtensions.cs
				return packageManager.GetPackages(language, currentFolder); 
			} 
		}

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Register license
			MapView.RegisterLicense(License, ApplicationContext);

			// Create package manager
			File packageFolder = new File(ApplicationContext.GetExternalFilesDir(null), "mappackages");

			if (!(packageFolder.Mkdir() || packageFolder.IsDirectory))
			{
				this.MakeToast("Could not create package folder!");
			}

			try
			{
				packageManager = new CartoPackageManager("nutiteq.osm", packageFolder.AbsolutePath);
			}
			catch (IOException e)
			{
				this.MakeToast("Exception: " + e);
				Finish();
			}

			packageManager.PackageManagerListener = PackageUpdateListener;

			packageManager.StartPackageListDownload();

			// Initialize ListView
			SetContentView(HelloMap.Resource.Layout.List);

			packageAdapter = new PackageManagerAdapter(this, HelloMap.Resource.Layout.package_item_row, packageArray);
			ListView.Adapter = packageAdapter;

			ActionBar.SetDisplayHomeAsUpEnabled(true);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			menu.Add("Map");

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnMenuItemSelected(int featureId, IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				OnBackPressed();
				return true;
			}

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

		#region Package update

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
				packageArray.AddRange(Packages);
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
			this.MakeToast("Error: " + e.ErrorType);
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
						PackageStatus status = packageManager.GetLocalPackageStatus(id, -1);
						pkg.UpdateStatus(status);


						packageArray[i] = pkg;
						packageAdapter.NotifyDataSetChanged();
					}
				}	
			});
		}

		#endregion

		#region Row Internal Button Click handling

		public void OnAdapterActionButtonClick(object sender, EventArgs e)
		{
			PackageManagerButton button = (PackageManagerButton)sender;
			System.Console.WriteLine("Clicked: " + button.PackageId + " - " + button.PackageName + " - " + button.Type);

			if (button.Type == PackageManagerButtonType.CancelPackageTasks)
			{
				packageManager.CancelPackageTasks(button.PackageId);
			}
			else if (button.Type == PackageManagerButtonType.SetPackagePriority)
			{
				packageManager.SetPackagePriority(button.PackageId, button.PriorityIndex);
			}
			else if (button.Type == PackageManagerButtonType.StartPackageDownload)
			{
				packageManager.StartPackageDownload(button.PackageId);

			}
			else if (button.Type == PackageManagerButtonType.StartRemovePackage)
			{
				packageManager.StartPackageRemove(button.PackageId);
			}
			else if (button.Type == PackageManagerButtonType.UpdatePackages)
			{
				currentFolder = currentFolder + button.PackageName + "/";
				UpdatePackages();
			}
		}

		#endregion
	}

	#region Supporting Classes

	/**
	* Holder class for packages containing views for each row in list view.
	*/
	public class PackageHolder : Java.Lang.Object
	{
		public TextView NameView { get; set; }

		public TextView StatusView { get; set; }

		public PackageManagerButton ActionButton { get; set; }
	}

	#endregion
}

