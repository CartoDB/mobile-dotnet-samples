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
            get { return "routing:" + Sources.NutiteqRouting; }
		}

		public const string PackageFolder = "com.carto.routingpackages." + Sources.NutiteqRouting;

		protected Marker startMarker, stopMarker;

		public RoutingService Service { get; set; }

		public MarkerStyle instructionUp, instructionLeft, instructionRight;

		public LocalVectorDataSource routeDataSource;
        public LocalVectorDataSource routeInstructionSource;
		public LocalVectorDataSource routeStartStopDataSource;

		public BalloonPopupStyleBuilder balloonBuilder;

		MapView MapView;
		
        public bool ShowTurns { get; set; } = true;

        public Routing(MapView map, string path) : base(path)
		{
			MapView = map;
            Projection = map.Options.BaseProjection;
		}

        public void BringLayersToFront()
        {
            MapView.Layers.Remove(routeLayer);
            MapView.Layers.Remove(routeInstructionLayer);
            MapView.Layers.Remove(startStopLayer);

            MapView.Layers.Add(routeLayer);
            MapView.Layers.Add(routeInstructionLayer);
            MapView.Layers.Add(startStopLayer);
        }

        public void Show(RoutingResult result)
		{
			routeDataSource.Clear();
            routeInstructionSource.Clear();

            var color = new Color(0, 122, 255, 225);
			Line line = CreatePolyline(result, color);
			routeDataSource.Add(line);

            // Add instruction markers
			RoutingInstructionVector instructions = result.Instructions;

            if (ShowTurns)
            {
				for (int i = 0; i < instructions.Count; i++)
				{
					RoutingInstruction instruction = instructions[i];
					MapPos position = result.Points[instruction.PointIndex];
                    CreateRoutePoint(position, instruction, routeInstructionSource);
				}
            }

		}

		public string GetMessage(RoutingResult result)
		{
			string distance = "Your route is " + (int)(result.TotalDistance / 100) / 10f + "km";
			string time = " (" + result.TotalTime.ConvertFromSecondsToHours() + ")";

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

        VectorLayer routeLayer, routeInstructionLayer, startStopLayer;

		public void SetSourcesAndElements(Bitmap olmarker, Bitmap up, Bitmap upleft, Bitmap upright, Color green, Color red, Color white)
		{
			// Define layer and datasource for route line
			routeDataSource = new LocalVectorDataSource(Projection);
			routeLayer = new VectorLayer(routeDataSource);
			MapView.Layers.Add(routeLayer);

            // Define layer and datasource for route instructions
            routeInstructionSource = new LocalVectorDataSource(Projection);
            routeInstructionLayer = new VectorLayer(routeInstructionSource);
            MapView.Layers.Add(routeInstructionLayer);

			// Define layer and datasource for route start and stop markers
			routeStartStopDataSource = new LocalVectorDataSource(Projection);
			// Initialize a vector layer with the previous data source
            startStopLayer = new VectorLayer(routeStartStopDataSource);
			// Add the previous vector layer to the map
			MapView.Layers.Add(startStopLayer);

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

		protected Line CreatePolyline(RoutingResult result, Color color)
		{
			LineStyleBuilder builder = new LineStyleBuilder();
			builder.Color = color;
            builder.Width = 10;

			return new Line(result.Points, builder.BuildStyle());
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
