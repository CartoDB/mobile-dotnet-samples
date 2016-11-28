
using System;
using System.Collections.Generic;
using System.IO;
using Carto.DataSources;
using Carto.PackageManager;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class PackageManagerController : UIViewController
	{
		public string Name { get { return "Advanced Package Manager"; } }

		public new string Description { get { return "Download packages from CARTO and manage them offline"; } }

		public static PackageManagerTileDataSource DataSource;

		CartoPackageManager packageManager;
		PackageListener packageListener;

		string currentFolder = ""; // Current 'folder' of the package, for example "Asia/"
		string language = "en"; // Language for the package names. Most major languages are supported

		readonly List<Package> packageList = new List<Package>();

		public List<Package> Packages { get { return packageManager.GetPackages(language, currentFolder); } }

		public PackageManagerController(string folder = null)
		{
			if (folder != null)
			{
				currentFolder = folder;
			}	
		}

		PackageManagerListView ContentView;

		MenuButton MenuButton { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Set packaged map controller button
			MenuButton = new MenuButton("icons/icon_map.png", new CGRect(0, 0, 30, 30));
			NavigationItem.RightBarButtonItem = MenuButton;

			// Create PackageManager instance for dealing with offline packages
			string folder = Utils.GetDocumentDirectory("regionpackages");

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
				Console.WriteLine("Directory: Does not exist... Creating");
			}
			else
			{
				Console.WriteLine("Directory: Exists");
			}

			packageManager = new CartoPackageManager("nutiteq.osm", folder);

			ContentView = new PackageManagerListView();
			View = ContentView;
			ContentView.AddRows(packageList);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			packageListener = new PackageListener();
			packageManager.PackageManagerListener = packageListener;

			// Always Attach handlers ViewWillAppear to avoid memory leaks and objects with multple handlers
			packageListener.OnPackageListUpdate += UpdatePackages;
			packageListener.OnPackageListFail += UpdatePackages;

			packageListener.OnPackageCancel += UpdatePackage;
			packageListener.OnPackageUpdate += UpdatePackage;
			packageListener.OnPackageStatusChange += UpdatePackage;
			packageListener.OnPackageFail += UpdatePackage;

			ContentView.ListSource.CellActionButtonClicked += OnCellActionButtonClick;

			packageManager.StartPackageListDownload();

			packageManager.Start();

			MenuButton.Click += OnMenuButtonClick;
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			// Always detach handlers ViewWillDisappear to avoid memory leaks and objects with multple handlers
			packageListener.OnPackageListUpdate -= UpdatePackages;
			packageListener.OnPackageListFail -= UpdatePackages;

			packageListener.OnPackageCancel -= UpdatePackage;
			packageListener.OnPackageUpdate -= UpdatePackage;
			packageListener.OnPackageStatusChange -= UpdatePackage;
			packageListener.OnPackageFail -= UpdatePackage;

			ContentView.ListSource.CellActionButtonClicked -= OnCellActionButtonClick;

			packageManager.Stop(true);
			packageListener = null;

			MenuButton.Click -= OnMenuButtonClick;
		}

		void OnMenuButtonClick(object sender, EventArgs e)
		{
			var controller = new PackagedMapController(packageManager);
			NavigationController.PushViewController(controller, true);
		}

		#region Package update

		void UpdatePackages(object sender, EventArgs e)
		{
			UpdatePackages();
		}

		void UpdatePackages()
		{
			InvokeOnMainThread(delegate
			{
				packageList.Clear();
				packageList.AddRange(Packages);
				ContentView.ListSource.Items = packageList;
				ContentView.ReloadData();
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
			InvokeOnMainThread(delegate
			{
				// Try to find the package that needs to be updated
				for (int i = 0; i < packageList.Count; i++)
				{
					Package pkg = packageList[i];

					if (id.Equals(pkg.Id))
					{
						PackageStatus status = packageManager.GetLocalPackageStatus(id, -1);
						pkg.UpdateStatus(status);

						packageList[i] = pkg;
						ContentView.ReloadData();
						break;
					}
				}
			});
		}

		#endregion

		#region Row Internal Button Click handling

		public void OnCellActionButtonClick(object sender, EventArgs e)
		{
			PackageManagerButton button = (PackageManagerButton)sender;

			if (button.Type == PMButtonType.CancelPackageTasks)
			{
				packageManager.CancelPackageTasks(button.PackageId);
			}
			else if (button.Type == PMButtonType.SetPackagePriority)
			{
				packageManager.SetPackagePriority(button.PackageId, button.PriorityIndex);
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
				var controller = new PackageManagerController(currentFolder + button.PackageName + "/");
				NavigationController.PushViewController(controller, true);
			}
		}

		#endregion
	}
}