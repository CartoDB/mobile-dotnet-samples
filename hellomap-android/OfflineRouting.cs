using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Carto.Layers;
using Carto.Core;
using Carto.Graphics;
using Carto.Ui;
using Carto.DataSources;
using Carto.VectorElements;
using Carto.Projections;
using Carto.Styles;
using Carto.PackageManager;
using Carto.VectorTiles;
using Carto.Geometry;
using Carto.Routing;
using System.Threading.Tasks;
using Android.Graphics;
using Android.Widget;
using Android.OS;
using HelloMap;

namespace CartoMobileSample
{
	[Activity (Label = "Offline Routing")]			
	public class OfflineRouting: BaseMapActivity
	{
		// Add packages you want to download
		internal static string[] downloadablePackages = new string[]{"EE-routing", "LV-routing", "LT-routing", "PL-routing"};

		static string ROUTING_PACKAGEMANAGER_SOURCE = "routing:nutiteq.osm.car";
		static string ROUTING_SERVICE_SOURCE = "nutiteq.osm.car";

		RoutingService onlineRoutingService;
		RoutingService offlineRoutingService;
		internal PackageManager packageManager;

		bool shortestPathRunning;
		internal bool offlinePackageReady;

		Marker startMarker;
		Marker stopMarker;

		MarkerStyle instructionUp;
		MarkerStyle instructionLeft;
		MarkerStyle instructionRight;

		LocalVectorDataSource routeDataSource;
		LocalVectorDataSource routeStartStopDataSource;
		BalloonPopupStyleBuilder balloonPopupStyleBuilder;

		protected override void OnCreate (Android.OS.Bundle bundle)
		{
			base.OnCreate (bundle);

			/// Set online base layer
			var styleAsset = AssetUtils.LoadAsset("nutibright-v2a.zip");
			var baseLayer = new CartoOnlineVectorTileLayer("nutiteq.osm", new ZippedAssetPackage(styleAsset));
			MapView.Layers.Add(baseLayer);

			// Create PackageManager instance for dealing with offline packages
			var packageFolder = new File (GetExternalFilesDir(null), "routingpackages");
			if (!(packageFolder.Mkdirs() || packageFolder.IsDirectory)) {
				Log.Fatal("Could not create package folder!");
			}

			packageManager = new CartoPackageManager(ROUTING_PACKAGEMANAGER_SOURCE, packageFolder.AbsolutePath);
			packageManager.PackageManagerListener = new RoutingPackageListener(this);
			packageManager.Start();

			// Fetch list of available packages from server. 
			// Note that this is asynchronous operation 
			// and listener will be notified via onPackageListUpdated when this succeeds.        
			packageManager.StartPackageListDownload();

			// create offline routing service connected to package manager
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
			RouteMapEventListener mapListener = new RouteMapEventListener(this);
			MapView.MapEventListener = mapListener;

			// Create markers for start & end, and a layer for them
			MarkerStyleBuilder markerStyleBuilder = new MarkerStyleBuilder();
			markerStyleBuilder.Bitmap = BitmapUtils
				.CreateBitmapFromAndroidBitmap(BitmapFactory.DecodeResource(
					Resources, Resource.Drawable.olmarker));
			markerStyleBuilder.HideIfOverlapped = false;
			markerStyleBuilder.Size = 30;

			markerStyleBuilder.Color = new Carto.Graphics.Color(Android.Graphics.Color.Green);

			startMarker = new Marker(new MapPos(0, 0), markerStyleBuilder.BuildStyle());
			startMarker.Visible = false;

			markerStyleBuilder.Color = new Carto.Graphics.Color(Android.Graphics.Color.Red);

			stopMarker = new Marker(new MapPos(0, 0), markerStyleBuilder.BuildStyle());
			stopMarker.Visible = false;

			routeStartStopDataSource.Add(startMarker);
			routeStartStopDataSource.Add(stopMarker);

			markerStyleBuilder.Color = new Carto.Graphics.Color(Android.Graphics.Color.White);
			markerStyleBuilder.Bitmap = BitmapUtils
				.CreateBitmapFromAndroidBitmap(BitmapFactory.DecodeResource(
					Resources, Resource.Drawable.direction_up));
			instructionUp = markerStyleBuilder.BuildStyle();

			markerStyleBuilder.Bitmap = BitmapUtils
				.CreateBitmapFromAndroidBitmap(BitmapFactory.DecodeResource(
					Resources, Resource.Drawable.direction_upthenleft));
			instructionLeft = markerStyleBuilder.BuildStyle();

			markerStyleBuilder.Bitmap = BitmapUtils
				.CreateBitmapFromAndroidBitmap(BitmapFactory.DecodeResource(
					Resources, Resource.Drawable.direction_upthenright));

			instructionRight = markerStyleBuilder.BuildStyle();

			// Style for instruction balloons
			balloonPopupStyleBuilder = new BalloonPopupStyleBuilder();
			balloonPopupStyleBuilder.TitleMargins = new BalloonPopupMargins(4,4,4,4);

			// Finally animate map to Estonia
			MapView.FocusPos = BaseProjection.FromWgs84(new MapPos(25.662893, 58.919365));
			MapView.Zoom = 7;

			Toast.MakeText(ApplicationContext, "Long-press on map to set route start and finish",ToastLength.Long).Show();
		}

		public void ShowRoute(MapPos startPos, MapPos stopPos) 
		{
			Log.Debug("calculating path " + startPos + " to " + stopPos);

			if (!offlinePackageReady) {
				RunOnUiThread (() => {
					string message = "Offline package is not ready, using online routing";
					Toast.MakeText(ApplicationContext, message,ToastLength.Long).Show();
				});
			}

			if (!shortestPathRunning) {
				shortestPathRunning = true;
				long timeStart;

				// run routing in background
				Task.Run (() => {
					timeStart = Java.Lang.JavaSystem.CurrentTimeMillis ();
					MapPosVector poses = new MapPosVector ();
					poses.Add (startPos);
					poses.Add (stopPos);
					RoutingRequest request = new RoutingRequest (BaseProjection, poses);
					RoutingResult result;
					if (offlinePackageReady) {
						result = offlineRoutingService.CalculateRoute (request);
					} else {
						result = onlineRoutingService.CalculateRoute (request);
					}

					// Now update response in UI thread
					RunOnUiThread (() => {
						
						if (result == null) {
							Toast.MakeText (ApplicationContext, "Routing failed", ToastLength.Long).Show ();
							shortestPathRunning = false;
							return;
						}

						string routeText = "The route is " + (int)(result.TotalDistance / 100) / 10f
						                   + "km (" + SecondsToHours ((int)result.TotalTime)
						                   + ") calculation: " + (Java.Lang.JavaSystem.CurrentTimeMillis () - timeStart) + " ms";
						Log.Info (routeText);

						Toast.MakeText (ApplicationContext, routeText, ToastLength.Long).Show ();

						routeDataSource.Clear ();

						startMarker.Visible = false;

						routeDataSource.Add (CreatePolyline (startMarker.Geometry
							.CenterPos, stopMarker.Geometry.CenterPos, result));

						// Add instruction markers
						RoutingInstructionVector instructions = result.Instructions;

						for (int i = 0; i < instructions.Count; i++) {
							RoutingInstruction instruction = instructions [i];
							// Log.d(Const.LOG_TAG, instruction.toString());
							CreateRoutePoint (result.Points [instruction.PointIndex], instruction.StreetName,
								instruction.Time, instruction.Distance, instruction.Action, routeDataSource);
						}

						shortestPathRunning = false;
					});
				});
			}
		}


		protected String SecondsToHours(int sec)
		{
			int hours = sec / 3600,
			remainder = sec % 3600,
			minutes = remainder / 60,
			seconds = remainder % 60;

			return ( (hours < 10 ? "0" : "") + hours
				+ "h" + (minutes < 10 ? "0" : "") + minutes
				+ "m" + (seconds< 10 ? "0" : "") + seconds+"s" );
		}

		protected void CreateRoutePoint(MapPos pos, String name,
			double time, double distance, RoutingAction action, LocalVectorDataSource ds) {

			MarkerStyle style = instructionUp;
			String str = "";

			switch (action) {
			case RoutingAction.RoutingActionHeadOn:
				str = "head on";
				break;
			case RoutingAction.RoutingActionFinish:
				str = "finish";
				break;
			case RoutingAction.RoutingActionTurnLeft:
				style = instructionLeft;
				str = "turn left";
				break;
			case RoutingAction.RoutingActionTurnRight:
				style = instructionRight;
				str = "turn right";
				break;
			case RoutingAction.RoutingActionUturn:
				str = "u turn";
				break;
			case RoutingAction.RoutingActionNoTurn:
			case RoutingAction.RoutingActionGoStraight:
				//            style = instructionUp;
				//            str = "continue";
				break;
			case RoutingAction.RoutingActionReachViaLocation:
				style = instructionUp;
				str = "stopover";
				break;
			case RoutingAction.RoutingActionEnterAgainstAllowedDirection:
				str = "enter against allowed direction";
				break;
			case RoutingAction.RoutingActionLeaveAgainstAllowedDirection:
				break;
			case RoutingAction.RoutingActionEnterRoundabout:
				str = "enter roundabout";
				break;
			case RoutingAction.RoutingActionStayOnRoundabout:
				str = "stay on roundabout";
				break;
			case RoutingAction.RoutingActionLeaveRoundabout:
				str = "leave roundabout";
				break;
			case RoutingAction.RoutingActionStartAtEndOfStreet:
				str = "start at end of street";
				break;
			}

			if (str != ""){
				Marker marker = new Marker(pos, style);
				BalloonPopup popup2 = new BalloonPopup(marker, balloonPopupStyleBuilder.BuildStyle(),
					str, "");
				ds.Add(popup2);
				ds.Add(marker);
			}
		}

		// Creates Nutiteq line from GraphHopper response
		protected Line CreatePolyline(MapPos start, MapPos end, RoutingResult result) 
		{
			LineStyleBuilder lineStyleBuilder = new LineStyleBuilder();
			lineStyleBuilder.Color = new Carto.Graphics.Color(Android.Graphics.Color.DarkGray);
			lineStyleBuilder.Width = 12;

			return new Line(result.Points, lineStyleBuilder.BuildStyle());
		}

		public void SetStartMarker(MapPos startPos) 
		{
			routeDataSource.Clear();
			stopMarker.Visible = false;
			startMarker.SetPos(startPos);
			startMarker.Visible = true;
		}

		public void SetStopMarker(MapPos pos) 
		{
			stopMarker.SetPos(pos);
			stopMarker.Visible = true;
		}
	}

	/**
	 * This MapListener waits for two clicks on map - first to set routing start point, and then
	 * second to mark end point and start routing service.
	 */
	public class RouteMapEventListener : MapEventListener
	{
		private MapPos startPos;
		private MapPos stopPos;
		private OfflineRouting controller;

		public RouteMapEventListener(OfflineRouting controller)
		{
			this.controller = controller;
		}

		// Map View manipulation handlers
		public override void OnMapClicked(MapClickInfo mapClickInfo)
		{
			if (mapClickInfo.ClickType == ClickType.ClickTypeLong)
			{
				MapPos clickPos = mapClickInfo.ClickPos;

				MapPos wgs84Clickpos = controller.BaseProjection.ToWgs84(clickPos);
				Log.Debug("onMapClicked " + wgs84Clickpos + " " + mapClickInfo.ClickType);

				if (startPos == null)
				{
					// set start, or start again
					startPos = clickPos;
					controller.SetStartMarker(clickPos);
				}
				else if (stopPos == null)
				{
					// set stop and calculate
					stopPos = clickPos;
					controller.SetStopMarker(clickPos);
					controller.ShowRoute(startPos, stopPos);

					// restart to force new route next time
					startPos = null;
					stopPos = null;
				}
			}
		}

		public override void OnMapMoved() 
		{
			
		}
	}


	/**
	 * Listener for package manager events. Contains only empty methods.
	 */
	class RoutingPackageListener : PackageManagerListener
	{
		private OfflineRouting controller;

		public RoutingPackageListener(OfflineRouting controller)
		{
			this.controller = controller;
		}

		public override void OnPackageListUpdated()
		{
			Log.Debug("Package list updated");

			var downloadedPackages = 0;
			var totalPackages = OfflineRouting.downloadablePackages.Length;
			for (int i = 0; i < totalPackages; i++)
			{
				var alreadyDownloaded = getPackageIfNotExists(OfflineRouting.downloadablePackages[i]);
				if (alreadyDownloaded)
				{
					downloadedPackages++;
				}
			}

			// If all downloaded, can start with offline routing
			if (downloadedPackages == totalPackages)
			{
				controller.offlinePackageReady = true;
			}
		}

		private bool getPackageIfNotExists(String packageId)
		{
			PackageStatus status = controller.packageManager.GetLocalPackageStatus(packageId, -1);
			if (status == null)
			{
				controller.packageManager.StartPackageDownload(packageId);
				return false;
			}
			else if (status.CurrentAction == PackageAction.PackageActionReady)
			{
				Log.Debug(packageId + " is downloaded and ready");
				return true;
			}

			return false;
		}

		public override void OnPackageListFailed()
		{
			Log.Error("Package list update failed");
		}

		public override void OnPackageStatusChanged(String id, int version, PackageStatus status)
		{
		}

		public override void OnPackageCancelled(String id, int version)
		{
		}

		public override void OnPackageUpdated(String id, int version)
		{
			Log.Debug("Offline package updated: " + id);
			controller.RunOnUiThread(() =>
			{
				Toast.MakeText(controller.BaseContext, "Offline package downloaded: " + id, ToastLength.Long).Show();
			});
			// if last downloaded
			if (id == OfflineRouting.downloadablePackages[OfflineRouting.downloadablePackages.Length - 1])
			{
				controller.offlinePackageReady = true;
			}
		}

		public override void OnPackageFailed(String id, int version, PackageErrorType errorType)
		{
			Log.Error("Offline package update failed: " + id);
		}
	}


}

