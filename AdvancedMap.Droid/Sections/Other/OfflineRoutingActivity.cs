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
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Offline routing", Description = "Offline routing with OpenStreetMap data packages")]
	public class OfflineRoutingActivity : MapBaseActivity
	{
		// Add packages you want to download
		internal static string[] downloadablePackages = { "EE-routing", "LV-routing" };

		const string ManagerSource = "routing:nutiteq.osm.car";
		const string ServiceSource = "nutiteq.osm.car";

		RoutingService onlineRoutingService, offlineRoutingService;
		internal PackageManager packageManager;

		bool shortestPathRunning;
		internal bool offlinePackageReady;

		Marker startMarker, stopMarker;

		MarkerStyle instructionUp, instructionLeft, instructionRight;

		LocalVectorDataSource routeDataSource;
		LocalVectorDataSource routeStartStopDataSource;
		BalloonPopupStyleBuilder balloonBuilder;

		RouteMapEventListener MapListener;
		RoutingPackageListener PackageListener;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleDefault);

			// Create PackageManager instance for dealing with offline packages
			var packageFolder = new File (GetExternalFilesDir(null), "routingpackages");
			if (!(packageFolder.Mkdirs() || packageFolder.IsDirectory)) {
				Log.Fatal("Could not create package folder!");
			}

			packageManager = new CartoPackageManager(ManagerSource, packageFolder.AbsolutePath);

			PackageListener = new RoutingPackageListener(packageManager, downloadablePackages);
			packageManager.PackageManagerListener = PackageListener;
			packageManager.Start();

			// Fetch list of available packages from server. 
			// Note that this is asynchronous operation 
			// and listener will be notified via onPackageListUpdated when this succeeds.        
			packageManager.StartPackageListDownload();

			// create offline routing service connected to package manager
			offlineRoutingService = new PackageManagerRoutingService(packageManager);

			// Create additional online routing service that will be used 
			// when offline package is not yet downloaded or offline routing fails
			onlineRoutingService = new CartoOnlineRoutingService(ServiceSource);

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
			MapListener = new RouteMapEventListener();
			MapView.MapEventListener = MapListener;

			// Create markers for start & end and a layer for them
			MarkerStyleBuilder markerBuilder = new MarkerStyleBuilder();
			markerBuilder.Bitmap = CreateBitmap(Resource.Drawable.olmarker);
			markerBuilder.HideIfOverlapped = false;
			markerBuilder.Size = 30;
			markerBuilder.Color = new Carto.Graphics.Color(Android.Graphics.Color.Green);

			startMarker = new Marker(new MapPos(0, 0), markerBuilder.BuildStyle());
			startMarker.Visible = false;

			markerBuilder.Color = new Carto.Graphics.Color(Android.Graphics.Color.Red);

			stopMarker = new Marker(new MapPos(0, 0), markerBuilder.BuildStyle());
			stopMarker.Visible = false;

			routeStartStopDataSource.Add(startMarker);
			routeStartStopDataSource.Add(stopMarker);

			markerBuilder.Color = new Carto.Graphics.Color(Android.Graphics.Color.White);
			markerBuilder.Bitmap = CreateBitmap(Resource.Drawable.direction_up);
			instructionUp = markerBuilder.BuildStyle();

			markerBuilder.Bitmap = CreateBitmap(Resource.Drawable.direction_upthenleft);
			instructionLeft = markerBuilder.BuildStyle();

			markerBuilder.Bitmap = CreateBitmap(Resource.Drawable.direction_upthenright);
			instructionRight = markerBuilder.BuildStyle();

			// Style for instruction balloons
			balloonBuilder = new BalloonPopupStyleBuilder();
			balloonBuilder.TitleMargins = new BalloonPopupMargins(4, 4, 4, 4);

			// Finally animate map to Estonia
			MapView.FocusPos = BaseProjection.FromWgs84(new MapPos(25.662893, 58.919365));
			MapView.Zoom = 7;

			Alert("Long-press on map to set route start and finish");
		}

		protected override void OnResume()
		{
			base.OnResume();

			MapListener.StartPositionClicked += OnStartPositionClick;
			MapListener.StopPositionClicked += OnStopPositionClick;

			PackageListener.OfflinePackageReady += OnOfflinePackageReady;
			PackageListener.PackageUpdated += OnPackageUpdated;
		}

		protected override void OnPause()
		{
			base.OnPause();

			MapListener.StartPositionClicked -= OnStartPositionClick;
			MapListener.StopPositionClicked -= OnStopPositionClick;

			PackageListener.OfflinePackageReady -= OnOfflinePackageReady;
			PackageListener.PackageUpdated -= OnPackageUpdated;
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
			RunOnUiThread(() =>
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
				RunOnUiThread(() =>
				{
					string message = "Offline package is not ready, using online routing";
					Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show();
				});
			}

			if (!shortestPathRunning)
			{
				shortestPathRunning = true;
				long timeStart;

				// run routing in background
				Task.Run(() =>
				{
					timeStart = Java.Lang.JavaSystem.CurrentTimeMillis();
					MapPosVector poses = new MapPosVector();

					poses.Add(startPos);
					poses.Add(stopPos);

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
					RunOnUiThread(() =>
					{
						if (result == null)
						{
							Alert("Routing failed");
							shortestPathRunning = false;
							return;
						}

						string distance = "The route is " + (int)(result.TotalDistance / 100) / 10f + "km";
						string time = "(" + result.TotalTime.ConvertFromSecondsToHours() + ")";
						string calculation = "| Calculation: " + (Java.Lang.JavaSystem.CurrentTimeMillis() - timeStart) + " ms";

						Alert(distance + time + calculation);

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
				BalloonPopup popup2 = new BalloonPopup(marker, balloonBuilder.BuildStyle(), str, "");

				source.Add(popup2);
				source.Add(marker);
			}
		}

		// Creates a line from GraphHopper response
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
}

