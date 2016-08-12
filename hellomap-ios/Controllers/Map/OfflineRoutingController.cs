
using System;
using System.IO;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorElements;
using UIKit;

namespace CartoMobileSample
{
	public class OfflineRoutingController : MapBaseController
	{
		public override string Name { get { return "Offline Routing"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use Carto Mobile SDK routing engine to calculate offline routes. " +
					"First a package is downloaded asynchronously. Once the package is available, routing works offline.";
			}
		}

		// Add packages you want to download
		internal static string[] downloadablePackages = { "EE-routing", "LV-routing", "LT-routing", "PL-routing" };

		const string ROUTING_PACKAGEMANAGER_SOURCE = "routing:nutiteq.osm.car";
		const string ROUTING_SERVICE_SOURCE = "nutiteq.osm.car";

		RoutingService onlineRoutingService;
		RoutingService offlineRoutingService;
		PackageManager packageManager;

		bool shortestPathRunning;
		bool offlinePackageReady;

		Marker startMarker;
		Marker stopMarker;

		MarkerStyle instructionUp;
		MarkerStyle instructionLeft;
		MarkerStyle instructionRight;

		LocalVectorDataSource routeDataSource;
		LocalVectorDataSource routeStartStopDataSource;
		BalloonPopupStyleBuilder balloonPopupStyleBuilder;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			/// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			// Create PackageManager instance for dealing with offline packages
			string folder = Utils.GetDocumentDirectory("routingpackages");

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
				Console.WriteLine("Directory: Does not exist... Creating");
			}
			else
			{
				Console.WriteLine("Directory: Exists");
			}

			packageManager = new CartoPackageManager(ROUTING_PACKAGEMANAGER_SOURCE, folder);
			//TODO
			//packageManager.PackageManagerListener = new RoutingPackageListener(this);
			packageManager.Start();

			// Fetch list of available packages from server. 
			// Note that this is asynchronous operation 
			// and listener will be notified via onPackageListUpdated when this succeeds.        
			packageManager.StartPackageListDownload();

			// Create offline routing service connected to package manager
			offlineRoutingService = new PackageManagerRoutingService(packageManager);

			// Create additional online routing service that will be used 
			// when offline package is not yet downloaded or offline routing fails
			onlineRoutingService = new CartoOnlineRoutingService(ROUTING_SERVICE_SOURCE);

			// Define layer and datasource for route line and instructions
			routeDataSource = new LocalVectorDataSource(BaseProjection);
			VectorLayer routeLayer = new VectorLayer(routeDataSource);
			MapView.Layers.Add(routeLayer);

			// Define layer and datasource for route start and stop markers
			routeStartStopDataSource = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer = new VectorLayer(routeStartStopDataSource);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer);

			// Set visible zoom range for the vector layer
			vectorLayer.VisibleZoomRange = new MapRange(0, 22);

			// Set route listener
			// TODO
			//RouteMapEventListener mapListener = new RouteMapEventListener(this);
			//MapView.MapEventListener = mapListener;

			// Create markers for start & end, and a layer for them
			MarkerStyleBuilder markerStyleBuilder = new MarkerStyleBuilder();
			markerStyleBuilder.Bitmap = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("olmarker.png"));
			markerStyleBuilder.HideIfOverlapped = false;
			markerStyleBuilder.Size = 30;

			markerStyleBuilder.Color = new Carto.Graphics.Color(0, 255, 0, 255);

			startMarker = new Marker(new MapPos(0, 0), markerStyleBuilder.BuildStyle());
			startMarker.Visible = false;

			markerStyleBuilder.Color = new Carto.Graphics.Color(255, 0, 0, 255);

			stopMarker = new Marker(new MapPos(0, 0), markerStyleBuilder.BuildStyle());
			stopMarker.Visible = false;

			routeStartStopDataSource.Add(startMarker);
			routeStartStopDataSource.Add(stopMarker);

			markerStyleBuilder.Color = new Carto.Graphics.Color(255, 255, 255, 255);
			markerStyleBuilder.Bitmap = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("direction_up.png"));
			instructionUp = markerStyleBuilder.BuildStyle();

			markerStyleBuilder.Bitmap = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("direction_upthenleft.png"));
			instructionLeft = markerStyleBuilder.BuildStyle();

			markerStyleBuilder.Bitmap = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("direction_upthenright.png"));

			instructionRight = markerStyleBuilder.BuildStyle();

			// Style for instruction balloons
			balloonPopupStyleBuilder = new BalloonPopupStyleBuilder();
			balloonPopupStyleBuilder.TitleMargins = new BalloonPopupMargins(4, 4, 4, 4);

			// Finally animate map to Estonia
			MapView.FocusPos = BaseProjection.FromWgs84(new MapPos(25.662893, 58.919365));
			MapView.Zoom = 7;

			Alert("Long-press on map to set route start and finish");
		}
	}
}

