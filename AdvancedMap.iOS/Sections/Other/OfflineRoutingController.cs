
using System;
using System.IO;
using System.Threading;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Routing;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorElements;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class OfflineRoutingController : MapBaseController
	{
		public override string Name { get { return "Offline Routing"; } }

		public override string Description
		{
			get
			{
				return "Routing engine to calculate offline routes. " +
					"Packages are downloaded and, once available, routing works offline";
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

		RouteMapEventListener MapListener;
		RoutingPackageListener PackageListener;

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
			PackageListener = new RoutingPackageListener(packageManager, downloadablePackages);

			packageManager.PackageManagerListener = PackageListener;

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

			MapListener = new RouteMapEventListener();
			MapView.MapEventListener = MapListener;

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

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			MapListener.StartPositionClicked += OnStartPositionClick;
			MapListener.StopPositionClicked += OnStopPositionClick;

			PackageListener.OfflinePackageReady += OnOfflinePackageReady;
			PackageListener.PackageUpdated += OnPackageUpdated;

			packageManager.Start();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			MapListener.StartPositionClicked -= OnStartPositionClick;
			MapListener.StopPositionClicked -= OnStopPositionClick;

			PackageListener.OfflinePackageReady -= OnOfflinePackageReady;
			PackageListener.PackageUpdated -= OnPackageUpdated;

			packageManager.Stop(true);
		}

		#region EventHandlers

		void OnStartPositionClick(object sender, RouteMapEventArgs e)
		{
			SetStartMarker(e.ClickPosition);
		}

		void OnStopPositionClick(object sender, RouteMapEventArgs e)
		{
			SetStopMarker(e.ClickPosition);
			ShowRoute(e.StartPosition, e.StopPosition);
		}

		void OnOfflinePackageReady(object sender, EventArgs e)
		{
			offlinePackageReady = true;
		}

		void OnPackageUpdated(object sender, PackageUpdateEventArgs e)
		{
			InvokeOnMainThread(() =>
			{
				Alert("Offline package downloaded: " + e.Id);
			});

			if (e.IsLastDownloaded)
			{
				offlinePackageReady = true;
			}
		}

		#endregion
		public void ShowRoute(MapPos startPos, MapPos stopPos)
		{
			Log.Debug("calculating path " + startPos + " to " + stopPos);

			if (!offlinePackageReady)
			{
				InvokeOnMainThread(() =>
				{
					string message = "Offline package is not ready, using online routing";
					Alert(message);
				});
			}

			if (!shortestPathRunning)
			{
				shortestPathRunning = true;
				long timeStart;

				// run routing in background
				System.Threading.Tasks.Task.Run(delegate
				{
					timeStart = DateTime.Now.ToUnixTime();
					MapPosVector poses = new MapPosVector { startPos, stopPos };

					RoutingRequest request = new RoutingRequest(BaseProjection, poses);
					RoutingResult result;

					if (offlinePackageReady)
					{
						result = offlineRoutingService.CalculateRoute(request);
					}
					else {
						result = onlineRoutingService.CalculateRoute(request);
					}

					// Now update response in UI thread
					InvokeOnMainThread(delegate
					{
						if (result == null)
						{
							Alert("Routing failed");
							shortestPathRunning = false;
							return;
						}

						routeDataSource.Clear();

						startMarker.Visible = false;

						Line line = CreatePolyline(startMarker.Geometry.CenterPos, stopMarker.Geometry.CenterPos, result);
						routeDataSource.Add(line);

						// Add instruction markers
						RoutingInstructionVector instructions = result.Instructions;

						for (int i = 0; i < instructions.Count; i++)
						{
							RoutingInstruction instruction = instructions[i];
							MapPos position = result.Points[instruction.PointIndex];
							CreateRoutePoint(position, instruction, routeDataSource);
						}

						string distance = "The route is " + (int)(result.TotalDistance / 100) / 10f + "km";
						string time = " (" + result.TotalTime.ConvertFromSecondsToHours() + ") ";
						string calculation = "| Calculation: " + (DateTime.Now.ToUnixTime() - timeStart) + " ms";

						Alert(distance + time + calculation);

						shortestPathRunning = false;
					});
				});
			}
		}

		protected void CreateRoutePoint(MapPos pos, RoutingInstruction instruction, LocalVectorDataSource source)
		{

			MarkerStyle style = instructionUp;
			string str = "";

			switch (instruction.Action)
			{
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
					//style = instructionUp;
					//str = "continue";
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

			if (str != "")
			{
				Marker marker = new Marker(pos, style);
				BalloonPopup popup2 = new BalloonPopup(marker, balloonPopupStyleBuilder.BuildStyle(), str, "");

				source.Add(popup2);
				source.Add(marker);
			}
		}

		// Creates Nutiteq line from GraphHopper response
		protected Line CreatePolyline(MapPos start, MapPos end, RoutingResult result)
		{
			LineStyleBuilder lineStyleBuilder = new LineStyleBuilder();

			Carto.Graphics.Color darkGray = new Carto.Graphics.Color(49, 79, 79, 255);
			lineStyleBuilder.Color = darkGray;
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
}

