using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Views;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Carto.Styles;
using Carto.VectorElements;

using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Offline routing", Description = "Offline routing with OpenStreetMap data packages")]
	public class OfflineRoutingActivity : BaseRoutingActivity
	{   
		PackageListener RoutingPackageListener { get; set; }

		CartoPackageManager RoutingPackageManager { get; set; }

		OfflineRoutingView ContentView;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ContentView = new OfflineRoutingView(this);
			SetContentView(ContentView);

			Initialize(ContentView.MapView);

			RoutingPackageManager = Routing.PackageManager;

			// Create offline routing service connected to package manager
            Routing.Service = new PackageManagerValhallaRoutingService(RoutingPackageManager);

			Alert("This sample uses an online map, but downloads routing packages");

			Alert("Click on the menu to see a list of countries that can be downloaded");
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				if (ContentView.Menu.IsVisible)
				{
					ContentView.Menu.Hide();
					return true;
				}

				OnBackPressed();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		public override void OnBackPressed()
		{
			if (ContentView.Menu.IsVisible)
			{
				ContentView.Menu.Hide();
				return;
			}

			base.OnBackPressed();
		}

		protected override void SetBaseLayer()
		{
            ContentView.MapView.AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleVoyager);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			RoutingPackageManager.Stop(true);
			RoutingPackageListener = null;
		}

		protected override void OnResume()
		{
			base.OnResume();

			RoutingPackageListener = new PackageListener();
			RoutingPackageManager.PackageManagerListener = RoutingPackageListener;

			RoutingPackageListener.OnPackageCancel += UpdatePackage;
			RoutingPackageListener.OnPackageUpdate += UpdatePackage;
			RoutingPackageListener.OnPackageStatusChange += UpdatePackage;
			RoutingPackageListener.OnPackageFail += UpdatePackage;

			// Just get the complete list of names from map package listener
			RoutingPackageListener.OnPackageListUpdate += UpdateRoutingPackages;
			RoutingPackageManager.Start();

			RoutingPackageManager.Start();
			RoutingPackageManager.StartPackageListDownload();

			ContentView.Button.Click += OnMenuButtonClicked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			RoutingPackageListener.OnPackageCancel -= UpdatePackage;
			RoutingPackageListener.OnPackageUpdate -= UpdatePackage;
			RoutingPackageListener.OnPackageStatusChange -= UpdatePackage;
			RoutingPackageListener.OnPackageFail -= UpdatePackage;

			RoutingPackageListener.OnPackageListUpdate -= UpdateRoutingPackages;

			RoutingPackageManager.Stop(true);
			RoutingPackageListener = null;

			ContentView.Button.Click -= OnMenuButtonClicked;
		}

		bool menuInitialized;

		void InitializeMenu()
		{
			// Fetch list of available packages from server. 
			// Note that this is asynchronous operation,
			// listener will be notified via onPackageListUpdated when this succeeds.
			RoutingPackageManager.StartPackageListDownload();

			menuInitialized = true;
		}

		void OnMenuButtonClicked(object sender, EventArgs e)
		{
			if (!menuInitialized)
			{
				// As we want user experience is to be as smooth as possible,
				// initialize the menu when it is actually clicked
				InitializeMenu();
			}

			if (ContentView.Menu.IsVisible)
			{
				ContentView.Menu.Hide();
			}
			else {
				ContentView.Menu.Show();
			}
		}

		public void OnAdapterActionButtonClick(object sender, EventArgs e)
		{
			PMButton button = (PMButton)sender;

			Console.WriteLine("Clicked: " + button.PackageId + " - " + button.PackageName + " - " + button.Type);

			if (button.Type == PMButtonType.CancelPackageTasks)
			{
				RoutingPackageManager.CancelPackageTasks(button.PackageId);
			}
			else if (button.Type == PMButtonType.SetPackagePriority)
			{
				RoutingPackageManager.SetPackagePriority(button.PackageId, button.PriorityIndex);
			}
			else if (button.Type == PMButtonType.StartPackageDownload)
			{
				RoutingPackageManager.StartPackageDownload(button.PackageId);
			}
			else if (button.Type == PMButtonType.StartRemovePackage)
			{
				RoutingPackageManager.StartPackageRemove(button.PackageId);
			}
			else if (button.Type == PMButtonType.UpdatePackages)
			{
				// Go to subfolder, however, this example has no foldering system.
			}
		}

		void UpdateRoutingPackages(object sender, EventArgs e)
		{
			RunOnUiThread(delegate
			{
				List<Package> packages = RoutingPackageManager.GetPackages();
				ContentView.UpdateList(packages);
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
			Alert("Error: " + e.ErrorType);
			UpdatePackage(e.Id);
		}

		void UpdatePackage(string id)
		{
			RunOnUiThread(delegate
			{
				ContentView.UpdatePackage(RoutingPackageManager, id);
			});
		}
	}
}
