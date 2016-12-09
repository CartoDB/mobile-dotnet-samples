using System;
using System.IO;

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
		PackageListener Listener { get; set; }

		CartoPackageManager Manager { get; set; }

		OfflineRoutingView ContentView;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ContentView = new OfflineRoutingView(this);
			SetContentView(ContentView);

			Manager = Routing.PackageManager;

			// Create offline routing service connected to package manager
			Routing.Service = new PackageManagerRoutingService(Manager);

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

		protected override void SetBaseLayer()
		{
			AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Manager.Stop(true);
			Listener = null;
		}

		protected override void OnResume()
		{
			base.OnResume();

			Listener = new PackageListener();
			Manager.PackageManagerListener = Listener;

			Listener.OnPackageCancel += UpdatePackage;
			Listener.OnPackageUpdate += UpdatePackage;
			Listener.OnPackageStatusChange += UpdatePackage;
			Listener.OnPackageFail += UpdatePackage;

			Listener.OnPackageListUpdate += UpdatePackages;

			Manager.Start();

			ContentView.Button.Click += OnMenuButtonClicked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			Listener.OnPackageCancel -= UpdatePackage;
			Listener.OnPackageUpdate -= UpdatePackage;
			Listener.OnPackageStatusChange -= UpdatePackage;
			Listener.OnPackageFail -= UpdatePackage;

			Listener.OnPackageListUpdate -= UpdatePackages;

			Manager.Stop(true);
			Listener = null;

			ContentView.Button.Click -= OnMenuButtonClicked;
		}

		bool menuInitialized;

		void InitializeMenu()
		{
			// Fetch list of available packages from server. 
			// Note that this is asynchronous operation,
			// listener will be notified via onPackageListUpdated when this succeeds.
			Manager.StartPackageListDownload();

			// Routing packages are as compact as possible,
			// so we create a second package manager to download region packages that contain names
			// This is only necessary for displaying them in a list. Download is by id
			var middleManager = new CartoPackageManager("nutiteq.osm", Routing.CreateFolder("regionpackages"));

			ContentView.UpdateList(middleManager.GetPackages());

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
				ContentView.Button.BringToFront();
			}
		}

		public void OnAdapterActionButtonClick(object sender, EventArgs e)
		{
			PMButton button = (PMButton)sender;

			button.SetAsMapPackage();

			Console.WriteLine("Clicked: " + button.PackageId + " - " + button.PackageName + " - " + button.Type);

			if (button.Type == PMButtonType.CancelPackageTasks)
			{
				Manager.CancelPackageTasks(button.PackageId);
			}
			else if (button.Type == PMButtonType.SetPackagePriority)
			{
				Manager.SetPackagePriority(button.PackageId, button.PriorityIndex);
			}
			else if (button.Type == PMButtonType.StartPackageDownload)
			{
				Manager.StartPackageDownload(button.PackageId);
			}
			else if (button.Type == PMButtonType.StartRemovePackage)
			{
				Manager.StartPackageRemove(button.PackageId);
			}
			else if (button.Type == PMButtonType.UpdatePackages)
			{
				// Go to subfolder, however, this example has no foldering system.
			}
		}

		void UpdatePackages(object sender, EventArgs e)
		{
			Console.WriteLine("UpdatePackages");
			ContentView.UpdateListWithRoutingPackages(Manager.GetPackages());
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
			RunOnUiThread(delegate
			{
				ContentView.UpdatePackage(Manager, id);
			});
		}
	}
}
