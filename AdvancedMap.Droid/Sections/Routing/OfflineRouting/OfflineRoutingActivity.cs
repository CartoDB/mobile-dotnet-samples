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
		internal static string[] downloadablePackages = { "EE-routing", "LV-routing" };

		RoutingPackageListener PackageListener { get; set; }

		CartoPackageManager Manager { get; set; }

		OfflineRoutingView ContentView;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			ContentView = new OfflineRoutingView(this);
			SetContentView(ContentView);

			Manager = Routing.PackageManager;

			PackageListener = new RoutingPackageListener(Manager, downloadablePackages);
			Manager.PackageManagerListener = PackageListener;

			Manager.Start();

			// Fetch list of available packages from server. 
			// Note that this is asynchronous operation 
			// and listener will be notified via onPackageListUpdated when this succeeds.        
			Manager.StartPackageListDownload();

			// Create offline routing service connected to package manager
			Routing.Service = new PackageManagerRoutingService(Manager);

			Alert("This sample uses an online map, but downloads routing packages");

			// Routing packages are as compact as possible,
			// so we create a second package manager to download region packages that contain names
			// This is only necessary for displaying them in a list. Download is by id
			var packageFolder = new Java.IO.File(ApplicationContext.GetExternalFilesDir(null), "regionpackages");
			var middleManager = new CartoPackageManager("nutiteq.osm", packageFolder.AbsolutePath);

			ContentView.UpdateList(middleManager.GetRoutingPackages());
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
			PackageListener = null;
		}

		protected override void OnResume()
		{
			base.OnResume();

			PackageListener.PackageUpdated += OnPackageUpdated;

			ContentView.Button.Click += OnMenuButtonClicked;
		}

		protected override void OnPause()
		{
			base.OnPause();

			PackageListener.PackageUpdated -= OnPackageUpdated;

			ContentView.Button.Click -= OnMenuButtonClicked;
		}

		void OnMenuButtonClicked(object sender, EventArgs e)
		{
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
			//Console.WriteLine("Clicked: " + button.PackageId + " - " + button.PackageName + " - " + button.Type);

			//if (button.Type == PMButtonType.CancelPackageTasks)
			//{
			//	packageManager.CancelPackageTasks(button.PackageId);
			//}
			//else if (button.Type == PMButtonType.SetPackagePriority)
			//{
			//	packageManager.SetPackagePriority(button.PackageId, button.PriorityIndex);
			//	UpdatePackages();
			//}
			//else if (button.Type == PMButtonType.StartPackageDownload)
			//{
			//	packageManager.StartPackageDownload(button.PackageId);
			//}
			//else if (button.Type == PMButtonType.StartRemovePackage)
			//{
			//	packageManager.StartPackageRemove(button.PackageId);
			//}
			//else if (button.Type == PMButtonType.UpdatePackages)
			//{
			//	currentFolder = currentFolder + button.PackageName + "/";
			//	UpdatePackages();
			//}
		}

		void OnPackageUpdated(object sender, PackageUpdateEventArgs e)
		{
			RunOnUiThread(() =>
			{
				Alert("Offline package downloaded: " + e.Id);
			});
		}
	}
}
