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
    public class Routing : BasePackageManagerClient
	{
		public override string Source
		{
            get { return "routing:" + Sources.CartoVector; }
		}

		public const string PackageFolder = "com.carto.routingpackages";

		protected Marker startMarker, stopMarker;

		public RoutingService Service { get; set; }

		public MarkerStyle instructionUp, instructionLeft, instructionRight;

		public LocalVectorDataSource routeDataSource;
		public LocalVectorDataSource routeStartStopDataSource;
		public BalloonPopupStyleBuilder balloonBuilder;

		MapView MapView;
		
        public bool ShowTurns { get; set; } = true;

        public Routing(MapView map, string path) : base(path)
		{
			MapView = map;
            Projection = map.Options.BaseProjection;
		}

		public void Show(RoutingResult result)
		{
			routeDataSource.Clear();

            var color = new Color(0, 122, 255, 150);
			Line line = CreatePolyline(startMarker.Geometry.CenterPos, stopMarker.Geometry.CenterPos, result, color);
			routeDataSource.Add(line);

			// Add instruction markers
			RoutingInstructionVector instructions = result.Instructions;

            if (ShowTurns)
            {
				for (int i = 0; i < instructions.Count; i++)
				{
					RoutingInstruction instruction = instructions[i];
					MapPos position = result.Points[instruction.PointIndex];
					CreateRoutePoint(position, instruction, routeDataSource);
				}
            }
		}

		public string GetMessage(RoutingResult result)
		{
			string distance = "Your route is " + (int)(result.TotalDistance / 100) / 10f + "km";
			string time = "(" + result.TotalTime.ConvertFromSecondsToHours() + ")";

			return distance + time;
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

            RoutingRequest request = new RoutingRequest(Projection, poses);

            RoutingResult result = null;

            try
            {
                result = Service.CalculateRoute(request);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception calculating route: " + e.Message);
            }

            return result;
        }

		public void SetSourcesAndElements(Bitmap olmarker, Bitmap up, Bitmap upleft, Bitmap upright, Color green, Color red, Color white)
		{
			// Define layer and datasource for route line and instructions
			routeDataSource = new LocalVectorDataSource(Projection);
			VectorLayer routeLayer = new VectorLayer(routeDataSource);
			MapView.Layers.Add(routeLayer);

			// Define layer and datasource for route start and stop markers
			routeStartStopDataSource = new LocalVectorDataSource(Projection);

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
            markerBuilder.Size = 20;
			instructionUp = markerBuilder.BuildStyle();

			markerBuilder.Bitmap = upleft;
			instructionLeft = markerBuilder.BuildStyle();

			markerBuilder.Bitmap = upright;
			instructionRight = markerBuilder.BuildStyle();
		}

        public void CreateRoutePoint(MapPos pos, RoutingInstruction instruction, LocalVectorDataSource source)
        {
            MarkerStyle style = instructionUp;

            if (instruction.Action == RoutingAction.RoutingActionTurnRight)
            {
                style = instructionRight;
            }
            else if (instruction.Action == RoutingAction.RoutingActionTurnLeft)
            {
                style = instructionLeft;
            }

            Marker marker = new Marker(pos, style);
            source.Add(marker);
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
