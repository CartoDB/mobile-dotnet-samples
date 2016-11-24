
using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorTiles;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Basic Package Manager", Description = "Download cities from an assortment of bounding boxes")]
	public class BasicPackageManagerActivity : BaseActivity
	{
		BasicPackageManagerView ContentView;

		CartoPackageManager manager;

		PackageListener packageListener;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Title = GetType().GetTitle();
			ActionBar.Subtitle = GetType().GetDescription();

			ContentView = new BasicPackageManagerView(this);
			SetContentView(ContentView);

			string folder = CreateFolder("citypackages");

			manager = new CartoPackageManager("nutiteq.osm", folder);

			ContentView.Menu.Items = BoundingBoxes.List;

			ContentView.SetBaseLayer(new PackageManagerTileDataSource(manager));

			Alert("Choose a city from the menu to download its bounding box");
		}

		protected override void OnResume()
		{
			base.OnResume();

			ContentView.Button.Click += OnMenuButtonClicked;
			ContentView.Menu.CityTapped += OnMenuSelectionChanged;

			packageListener = new PackageListener();
			manager.PackageManagerListener = packageListener;

			// Always Attach handlers OnResume to avoid memory leaks and objects with multple handlers
			packageListener.OnPackageCancel += UpdatePackage;
			packageListener.OnPackageUpdate += UpdatePackage;
			packageListener.OnPackageStatusChange += UpdatePackage;
			packageListener.OnPackageFail += UpdatePackage;

			manager.Start();
		}

		protected override void OnPause()
		{

			ContentView.Button.Click -= OnMenuButtonClicked;
			ContentView.Menu.CityTapped -= OnMenuSelectionChanged;

			// Always detach handlers OnPause to avoid memory leaks and objects with multple handlers
			packageListener.OnPackageCancel -= UpdatePackage;
			packageListener.OnPackageUpdate -= UpdatePackage;
			packageListener.OnPackageStatusChange -= UpdatePackage;
			packageListener.OnPackageFail -= UpdatePackage;

			manager.Stop(true);
			packageListener = null;

			base.OnPause();
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

		void OnMenuSelectionChanged(object sender, EventArgs e)
		{
			ContentView.ZoomOut();

			BoundingBox city = (BoundingBox)sender;
			ContentView.Menu.Hide();
			DownloadAndZoomToBbox(city);
		}

		BoundingBox currentCity;

		void DownloadAndZoomToBbox(BoundingBox bbox)
		{
			currentCity = bbox;

			string packaged = bbox.ToString();

			// Package version has no use here, can be anything
			if (manager.GetLocalPackageStatus(packaged, 1) == null)
			{
				manager.StartPackageDownload(packaged);
				Alert("Downloading " + bbox.Name);
			}
			else {
				UpdateStatusLabel("Download complete");
				Alert(bbox.Name + " already downloaded. Zooming");
				ContentView.ZoomTo(bbox.Center);
			}
		}

		void UpdatePackage(object sender, PackageEventArgs e)
		{
			UpdateStatusLabel("Downloaded Complete");
			ContentView.ZoomTo(currentCity.Center);
		}

		void UpdatePackage(object sender, PackageStatusEventArgs e)
		{
			UpdateStatusLabel("Progress: " + e.Status.Progress + "%");
		}

		void UpdatePackage(object sender, PackageFailedEventArgs e)
		{
			UpdateStatusLabel("Failed: " + e.ErrorType);
		}

		string CreateFolder(string name)
		{
			string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), name);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return path;
		}

		void UpdateStatusLabel(string text)
		{
			RunOnUiThread(delegate { ContentView.StatusLabel.Text = text; });
		}

	}
}
