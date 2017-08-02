using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Carto.Core;
using Carto.DataSources;
using Carto.PackageManager;
using Carto.Ui;
using Java.IO;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "Advanced Package Manager", Description = "Download countries for offline use")]
	public class AdvancedPackageManagerActivity : ListActivity
	{
		public CartoPackageManager packageManager;
		PackageManagerAdapter packageAdapter;
		List<Package> packageArray = new List<Package>();

		string currentFolder = ""; // Current 'folder' of the package, for example "Asia/"
		string language = "en"; // Language for the package names. Most major languages are supported

		PackageListener packageListener;

		public List<Package> Packages { 
			get {
				// Extension method. cf CommonMapExtensions.cs
				return packageManager.GetPackages(language, currentFolder); 
			} 
		}

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ActionBar.SetDisplayHomeAsUpEnabled(true);
			ActionBar.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable { Color = Colors.ActionBar });

			Title = GetType().GetTitle();

			// Create package manager
			File packageFolder = new File(ApplicationContext.GetExternalFilesDir(null), "regionpackages");

			if (!(packageFolder.Mkdir() || packageFolder.IsDirectory))
			{
				this.MakeToast("Could not create package folder!");
			}

			try
			{
                packageManager = new CartoPackageManager(Sources.CartoVector, packageFolder.AbsolutePath);
			}
			catch (IOException e)
			{
				this.MakeToast("Exception: " + e);
				Finish();
			}

			// Initialize ListView
			SetContentView(Resource.Layout.List);
			packageAdapter = new PackageManagerAdapter(this, ListView, Resource.Layout.package_item_row, packageArray);
			ListView.Adapter = packageAdapter;
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			IMenuItem item = menu.Add("Map");
			item.SetIcon(Android.Resource.Drawable.IcDialogMap);
			item.SetOnMenuItemClickListener(new MenuClickListener(this));
			if (Build.VERSION.SdkInt > BuildVersionCodes.Honeycomb)
			{
				item.SetShowAsAction(ShowAsAction.Always);
			}

			return base.OnCreateOptionsMenu(menu);
		}

		public override bool OnMenuItemSelected(int featureId, IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				OnBackPressed();
				return true;
			}

			return base.OnMenuItemSelected(featureId, item);
		}

		protected override void OnResume()
		{
			base.OnResume();

			packageListener = new PackageListener();
			packageManager.PackageManagerListener = packageListener;

			// Always Attach handlers OnResume to avoid memory leaks and objects with multple handlers
			packageListener.OnPackageListUpdate += UpdatePackages;
			packageListener.OnPackageListFail += UpdatePackages;

			packageListener.OnPackageCancel += UpdatePackage;
			packageListener.OnPackageUpdate += UpdatePackage;
			packageListener.OnPackageStatusChange += UpdatePackage;
			packageListener.OnPackageFail += UpdatePackage;

			packageManager.StartPackageListDownload();
			packageManager.Start();
		}

		protected override void OnPause()
		{
			base.OnPause();

			// Always detach handlers OnPause to avoid memory leaks and objects with multple handlers
			packageListener.OnPackageListUpdate -= UpdatePackages;
			packageListener.OnPackageListFail -= UpdatePackages;

			packageListener.OnPackageCancel -= UpdatePackage;
			packageListener.OnPackageUpdate -= UpdatePackage;
			packageListener.OnPackageStatusChange -= UpdatePackage;
			packageListener.OnPackageFail -= UpdatePackage;

			packageManager.Stop(true);
			packageListener = null;
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
						packageAdapter.Update(pkg);
					}
				}	
			});
		}

		#endregion

		#region Row Internal Button Click handling

		public void OnAdapterActionButtonClick(object sender, EventArgs e)
		{
			PMButton button = (PMButton)sender;
			System.Console.WriteLine("Clicked: " + button.PackageId + " - " + button.PackageName + " - " + button.Type);

			if (button.Type == PMButtonType.CancelPackageTasks)
			{
				packageManager.CancelPackageTasks(button.PackageId);
			}
			else if (button.Type == PMButtonType.SetPackagePriority)
			{
				packageManager.SetPackagePriority(button.PackageId, button.PriorityIndex);
				UpdatePackages();
			}
			else if (button.Type == PMButtonType.StartPackageDownload)
			{
				packageManager.StartPackageDownload(button.PackageId);
			}
			else if (button.Type == PMButtonType.StartRemovePackage)
			{
				packageManager.StartPackageRemove(button.PackageId);
			}
			else if (button.Type == PMButtonType.UpdatePackages)
			{
				currentFolder = currentFolder + button.PackageName + "/";
				UpdatePackages();
			}
		}

		#endregion
	}
}

