using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Carto.Core;
using Carto.Ui;
using Carto.Utils;
using Carto.Layers;
using Carto.DataSources;
using Carto.VectorElements;
using Carto.Projections;
using Carto.Styles;
using Carto.VectorTiles;
using Carto.Graphics;
using Carto.Geometry;

namespace CartoMobileSample
{
	partial class MainViewController : GLKit.GLKViewController
	{
		public MainViewController(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// GLKViewController-specific parameters for smoother animations
			ResumeOnDidBecomeActive = false;
			PreferredFramesPerSecond = 60;

			// Register license
			Log.ShowError = true;
			Log.ShowWarn = true;
			Log.ShowDebug = true;
			MapView.RegisterLicense(AppDelegate.License);

			// Create package manager folder (Platform-specific)
			var paths = NSSearchPath.GetDirectories(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User);
			var packagesDir = paths[0] + "packages";
			NSFileManager.DefaultManager.CreateDirectory(packagesDir, true, null);


			// Initialize map
			string downloadArea = "bbox(-0.8164,51.2382,0.6406,51.7401)"; // London (about 30MB)
//			string downloadId = "EE"; // one of ID-s from https://developer.nutiteq.com/guides/packages
																		  
		    // Decide what to download offline
			var toBeDownloaded = downloadArea;
			string importPackagePath = AssetUtils.CalculateResourcePath("world_ntvt_0_4.mbtiles");
			//	MapSetup.InitializePackageManager (packagesDir, importPackagePath, Map, toBeDownloaded);

			/// Online vector base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			Map.Layers.Add(baseLayer);

			MapSetup.AddMapOverlays(Map);

			var json = System.IO.File.ReadAllText(AssetUtils.CalculateResourcePath("capitals_3857.geojson"));

			MapSetup.AddJsonLayer (Map, json);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// GLKViewController-specific, do on-demand rendering instead of constant redrawing
			// This is VERY IMPORTANT as it stops battery drain when nothing changes on the screen!
			Paused = true;
		}

	}
}
