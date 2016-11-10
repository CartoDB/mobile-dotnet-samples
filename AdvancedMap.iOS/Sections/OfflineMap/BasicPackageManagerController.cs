
using System;
using System.IO;
using Carto.Core;
using Carto.PackageManager;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class BasicPackageManagerController : MapBaseController
	{
		public override string Name { get { return "Basic Package Manager"; } }

		public override string Description { get { return "Download a bounding box of London"; } }

		CartoPackageManager packageManager;

		PackageListener PackageUpdateListener = new PackageListener();

		UILabel status;

		BoundingBox bbox;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			AddBaseLayer(Carto.Layers.CartoBaseMapStyle.CartoBasemapStyleDefault);

			string folder = CreateFolder("mappackages");

			SetStatusLabel();

			packageManager = new CartoPackageManager("nutiteq.osm", folder);
			packageManager.PackageManagerListener = PackageUpdateListener;

			// Custom convience class to enhance readability 
			bbox = new BoundingBox { MinLon = -0.8164, MinLat = 51.2382, MaxLon = 0.6406, MaxLat = 51.7401 };

			if (packageManager.GetLocalPackage(bbox.ToString()) == null)
			{
				packageManager.StartPackageDownload(bbox.ToString());
			}
			else {
				UpdateStatusLabel("Package downloaded");
				ZoomTo(bbox.Center);
			}
		}

		void ZoomTo(MapPos position)
		{
			MapView.FocusPos = BaseProjection.FromWgs84(position);
			MapView.SetZoom(12, 2);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// Always Attach handlers ViewWillAppear to avoid memory leaks and objects with multple handlers
			PackageUpdateListener.OnPackageCancel += UpdatePackage;
			PackageUpdateListener.OnPackageUpdate += UpdatePackage;
			PackageUpdateListener.OnPackageStatusChange += UpdatePackage;
			PackageUpdateListener.OnPackageFail += UpdatePackage;

			packageManager.Start();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			// Always detach handlers ViewWillDisappear to avoid memory leaks and objects with multple handlers
			PackageUpdateListener.OnPackageCancel -= UpdatePackage;
			PackageUpdateListener.OnPackageUpdate -= UpdatePackage;
			PackageUpdateListener.OnPackageStatusChange -= UpdatePackage;
			PackageUpdateListener.OnPackageFail -= UpdatePackage;

			packageManager.Stop(true);
		}

		void UpdatePackage(object sender, PackageEventArgs e)
		{
			UpdateStatusLabel("Downloaded Complete");
			ZoomTo(bbox.Center);
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
			status.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 150);
			status.TextColor = UIColor.White;
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
	}
}
