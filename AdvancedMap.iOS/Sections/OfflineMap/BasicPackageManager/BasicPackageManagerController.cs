
using System;
using System.IO;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorTiles;
using CoreGraphics;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class BasicPackageManagerController : MapBaseController
	{
		public override string Name { get { return "Basic Package Manager"; } }

		public override string Description { get { return "Download city maps for offline use"; } }

		CartoPackageManager packageManager;

		PackageListener packageListener;

		UILabel status;

		public CityChoiceMenu Menu { get; set; }
		MenuButton MenuButton { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Menu = new CityChoiceMenu();
			Menu.Items = BoundingBoxes.List;

			MenuButton = new MenuButton("icons/icon_more.png", new CGRect(0, 10, 20, 30));
			NavigationItem.RightBarButtonItem = MenuButton;

			string folder = CreateFolder("citypackages");

			SetStatusLabel();

			packageManager = new CartoPackageManager("nutiteq.osm", folder);

			SetBaseLayer();
		}

		void ZoomTo(MapPos position)
		{
			MapView.FocusPos = BaseProjection.FromWgs84(position);
			MapView.SetZoom(12, 2);
		}

		void ZoomOut()
		{
			MapView.SetZoom(0, 2);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// Always Attach handlers ViewWillAppear to avoid memory leaks and objects with multple handlers

			MenuButton.Click += OnMenuButtonClick;
			Menu.CityTapped += OnMenuSelectionChanged;

			packageListener = new PackageListener();
			packageManager.PackageManagerListener = packageListener;

			packageListener.OnPackageCancel += UpdatePackage;
			packageListener.OnPackageUpdate += UpdatePackage;
			packageListener.OnPackageStatusChange += UpdatePackage;
			packageListener.OnPackageFail += UpdatePackage;

			packageManager.Start();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			// Always detach handlers ViewWillDisappear to avoid memory leaks and objects with multple handlers

			MenuButton.Click -= OnMenuButtonClick;
			Menu.CityTapped -= OnMenuSelectionChanged;

			packageListener = new PackageListener();
			packageManager.PackageManagerListener = packageListener;

			packageListener.OnPackageCancel -= UpdatePackage;
			packageListener.OnPackageUpdate -= UpdatePackage;
			packageListener.OnPackageStatusChange -= UpdatePackage;
			packageListener.OnPackageFail -= UpdatePackage;

			packageManager.Stop(true);
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			Alert("Choose a city from the menu to download its bounding box");
		}

		BoundingBox currentCity;

		void DownloadAndZoomToBbox(BoundingBox bbox)
		{
			currentCity = bbox;

			string packaged = bbox.ToString();

			// Package version has no use here, can be anything
			if (packageManager.GetLocalPackageStatus(packaged, 1) == null)
			{
				packageManager.StartPackageDownload(packaged);
				Alert("Downloading " + bbox.Name);
			}
			else {
				UpdateStatusLabel("Download complete");
				Alert(bbox.Name + " already downloaded. Zooming");
				ZoomTo(bbox.Center);
			}
		}

		void OnMenuButtonClick(object sender, EventArgs e)
		{
			if (Menu.IsVisible)
			{
				Menu.Hide();
			}
			else {
				Menu.Show();
			}
		}

		void OnMenuSelectionChanged(object sender, EventArgs e)
		{
			ZoomOut();

			BoundingBox city = (BoundingBox)sender;
			Menu.Hide();
			DownloadAndZoomToBbox(city);
		}

		void UpdatePackage(object sender, PackageEventArgs e)
		{
			UpdateStatusLabel("Downloaded Complete");
			ZoomTo(currentCity.Center);
		}

		void UpdatePackage(object sender, PackageStatusEventArgs e)
		{
			UpdateStatusLabel("Progress: " + e.Status.Progress + "%");
		}

		void UpdatePackage(object sender, PackageFailedEventArgs e)
		{
			UpdateStatusLabel("Failed: " + e.ErrorType.ToString());
		}

		string CreateFolder(string name)
		{
			string folder = Utils.GetDocumentDirectory("mappackages");

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			return folder;
		}

		void UpdateStatusLabel(string text)
		{
			InvokeOnMainThread(delegate { status.Text = text; });
		}

		void SetStatusLabel()
		{
			status = new UILabel();
			status.BackgroundColor = UIColor.FromRGBA(255, 255, 255, 160);
			status.TextColor = UIColor.Black;
			status.Layer.CornerRadius = 5;
			status.ClipsToBounds = true;
			status.TextAlignment = UITextAlignment.Center;
			status.Lines = 0;
			status.LineBreakMode = UILineBreakMode.WordWrap;
			status.Font = UIFont.FromName("HelveticaNeue", 12);

			MapView.AddSubview(status);

			CoreGraphics.CGRect screen = UIScreen.MainScreen.Bounds;

			nfloat width = screen.Width / 2;
			nfloat height = width / 4;

			nfloat x = screen.Width / 2 - width / 2;
			nfloat y = UIApplication.SharedApplication.StatusBarFrame.Height + NavigationController.NavigationBar.Frame.Height + 10;

			status.Frame = new CoreGraphics.CGRect(x, y, width, height);
		}

		public void SetBaseLayer()
		{
			var layer = new CartoOfflineVectorTileLayer(packageManager, CartoBaseMapStyle.CartoBasemapStyleDefault);
			MapView.Layers.Add(layer);
		}
	}
}
