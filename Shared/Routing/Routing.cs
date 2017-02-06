using System;
using System.IO;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.PackageManager;
using Carto.Projections;
using Carto.Routing;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared
{
	public class Routing
	{
		public const string ServiceSource = "nutiteq.osm.car";

		const string ManagerSource = "routing:nutiteq.osm.car";

		protected Marker startMarker, stopMarker;

		public RoutingService Service { get; set; }

		public MarkerStyle instructionUp, instructionLeft, instructionRight;

		public LocalVectorDataSource routeDataSource;
		public LocalVectorDataSource routeStartStopDataSource;
		public BalloonPopupStyleBuilder balloonBuilder;

		// Package manager is only used in offline routing
		public virtual CartoPackageManager PackageManager
		{
			get
			{
				// Create PackageManager instance for dealing with offline packages
				string folder = CreateFolder();
				CartoPackageManager manager = new CartoPackageManager(ManagerSource, folder);
				return manager;
			}
		}

		MapView MapView;
		Projection BaseProjection;

		public Routing(MapView map, Projection projection)
		{
			MapView = map;
			BaseProjection = projection;
		}

		public void Show(RoutingResult result, Color lineColor)
		{
			routeDataSource.Clear();

			startMarker.Visible = false;

			Line line = CreatePolyline(startMarker.Geometry.CenterPos, stopMarker.Geometry.CenterPos, result, lineColor);
			routeDataSource.Add(line);

			// Add instruction markers
			RoutingInstructionVector instructions = result.Instructions;

			for (int i = 0; i < instructions.Count; i++)
			{
				RoutingInstruction instruction = instructions[i];
				MapPos position = result.Points[instruction.PointIndex];
				CreateRoutePoint(position, instruction, routeDataSource);
			}
		}

		public string GetMessage(RoutingResult result, long start, long current)
		{

			string distance = "The route is " + (int)(result.TotalDistance / 100) / 10f + "km";
			string time = "(" + result.TotalTime.ConvertFromSecondsToHours() + ")";
			string calculation = " | Calculation: " + (current - start) + " ms";

			return distance + time + calculation;
		}

		public string CreateFolder()
		{
			return CreateFolder("routingpackages");
		}

		public string CreateFolder(string name)
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), name);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			return path;
		}

		public RoutingResult GetResult(MapPos startPos, MapPos stopPos)
		{
			MapPosVector poses = new MapPosVector();

			poses.Add(startPos);
			poses.Add(stopPos);

			RoutingRequest request = new RoutingRequest(BaseProjection, poses);

			return Service.CalculateRoute(request);
		}

		public void SetSourcesAndElements(Bitmap olmarker, Bitmap up, Bitmap upleft, Bitmap upright, Color green, Color red, Color white)
		{
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

			// Create markers for start & end and a layer for them
			MarkerStyleBuilder markerBuilder = new MarkerStyleBuilder();
			markerBuilder.Bitmap = olmarker;
			markerBuilder.HideIfOverlapped = false;
			markerBuilder.Size = 30;
			markerBuilder.Color = green;

			startMarker = new Marker(new MapPos(0, 0), markerBuilder.BuildStyle());
			startMarker.Visible = false;

			markerBuilder.Color = red;

			stopMarker = new Marker(new MapPos(0, 0), markerBuilder.BuildStyle());
			stopMarker.Visible = false;

			routeStartStopDataSource.Add(startMarker);
			routeStartStopDataSource.Add(stopMarker);

			markerBuilder.Color = white;
			markerBuilder.Bitmap = up;
			instructionUp = markerBuilder.BuildStyle();

			markerBuilder.Bitmap = upleft;
			instructionLeft = markerBuilder.BuildStyle();

			markerBuilder.Bitmap = upright;
			instructionRight = markerBuilder.BuildStyle();

			// Style for instruction balloons
			balloonBuilder = new BalloonPopupStyleBuilder();
			balloonBuilder.TitleMargins = new BalloonPopupMargins(4, 4, 4, 4);
		}

		public void CreateRoutePoint(MapPos pos, RoutingInstruction instruction, LocalVectorDataSource source)
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
		protected Line CreatePolyline(MapPos start, MapPos end, RoutingResult result, Color color)
		{
			LineStyleBuilder lineStyleBuilder = new LineStyleBuilder();
			lineStyleBuilder.Color = color;
			lineStyleBuilder.Width = 7;

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
