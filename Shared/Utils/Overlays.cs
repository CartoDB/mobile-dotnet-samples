using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using Carto.Utils;
using Carto.VectorElements;

namespace Shared
{
	public class Overlays
	{
		public static void AddPoint1(LocalVectorDataSource source, Projection projection)
		{
			Color green = new Color(0, 255, 0, 255);
			Point point1 = GetPoint(projection, new MapPos(24.651488, 59.423581), green);
			point1.SetMetaDataElement("ClickText", new Variant("Point nr 1"));
			source.Add(point1);
		}

		public static void AddPoint2(LocalVectorDataSource source, Projection projection)
		{
			Color red = new Color(255, 0, 0, 255);
			Point point2 = GetPoint(projection, new MapPos(24.655994, 59.422716), red);
			point2.SetMetaDataElement("ClickText", new Variant("Point nr 2"));
			source.Add(point2);
		}

		public static void AddOverlyingLines(MapView map, LocalVectorDataSource source, Projection projection)
		{
			// Initialize a second vector data source and vector layer
			// This secondary vector layer will be used for drawing borders for
			// line elements (by drawing the same line twice, with different widths)
			// Drawing order withing a layer is currently undefined
			// Using multiple layers is the only way to guarantee
			// that point, line and polygon elements are drawn in a specific order
			LocalVectorDataSource source2 = new LocalVectorDataSource(projection);
			VectorLayer vectorLayer2 = new VectorLayer(source2);
			map.Layers.Add(vectorLayer2);

			vectorLayer2.VisibleZoomRange = new MapRange(10, 24);

			// Create line style, and line poses
			LineStyleBuilder lineStyleBuilder = new LineStyleBuilder();
			lineStyleBuilder.Color = new Color(255, 255, 255, 255); // White
			lineStyleBuilder.Width = 8;

			MapPosVector linePoses = new MapPosVector();

			linePoses.Add(projection.FromWgs84(new MapPos(24.645565, 59.422074)));
			linePoses.Add(projection.FromWgs84(new MapPos(24.643076, 59.420502)));
			linePoses.Add(projection.FromWgs84(new MapPos(24.645351, 59.419149)));
			linePoses.Add(projection.FromWgs84(new MapPos(24.648956, 59.420393)));
			linePoses.Add(projection.FromWgs84(new MapPos(24.650887, 59.422707)));

			// Add first line
			Line line1 = new Line(linePoses, lineStyleBuilder.BuildStyle());
			line1.SetMetaDataElement("ClickText", new Variant("Line nr 1"));
			source2.Add(line1);

			// Create another line style, use the same lines poses
			lineStyleBuilder = new LineStyleBuilder();
			lineStyleBuilder.Color = new Color(204, 15, 0, 255);
			lineStyleBuilder.Width = 12;

			// Add second line to the second layer.
			Line line2 = new Line(linePoses, lineStyleBuilder.BuildStyle());
			line2.SetMetaDataElement("ClickText", new Variant("Line nr 2"));
			source.Add(line2);
		}

		public static void AddText1(LocalVectorDataSource source, Projection projection)
		{
			// Create text style
			TextStyleBuilder builder = new TextStyleBuilder();
			builder.Color = new Color(255, 0, 0, 255); // Red
			builder.OrientationMode = BillboardOrientation.BillboardOrientationFaceCamera;

			// This enables higher resolution texts for retina devices, but consumes more memory and is slower
			builder.ScaleWithDPI = false;

			// Add text
			MapPos position = projection.FromWgs84(new MapPos(24.653302, 59.422269));
			Text popup = new Text(position, builder.BuildStyle(), "Face camera text");

			popup.SetMetaDataElement("ClickText", new Variant("Text nr 1"));
			source.Add(popup);
		}

		public static void AddText2(LocalVectorDataSource source, Projection projection)
		{
			TextStyleBuilder builder = new TextStyleBuilder();
			builder.OrientationMode = BillboardOrientation.BillboardOrientationFaceCamera;

			MapPos position = projection.FromWgs84(new MapPos(24.633216, 59.426869));
			Text popup = new Text(position, builder.BuildStyle(), "Face camera ground text");
			popup.SetMetaDataElement("ClickText", new Variant("Text nr 2"));

			source.Add(popup);
		}

		public static void AddText3(LocalVectorDataSource source, Projection projection)
		{

			TextStyleBuilder builder = new TextStyleBuilder();
			builder.FontSize = 22;
			builder.OrientationMode = BillboardOrientation.BillboardOrientationGround;

			MapPos position = projection.FromWgs84(new MapPos(24.646457, 59.420839));
			Text popup = new Text(position, builder.BuildStyle(), "Ground text");
			popup.SetMetaDataElement("ClickText", new Variant("Text nr 3"));

			source.Add(popup);
		}

		public static void AddBalloonPopup1(LocalVectorDataSource source, Projection projection, Bitmap leftImage, Bitmap rightImage)
		{
			// TODO REMOVE WHEN ANDROID COMPLETE
			//Bitmap infoImage = BitmapFactory.decodeResource(getResources(), R.drawable.info);
			//Bitmap arrowImage = BitmapFactory.decodeResource(getResources(), R.drawable.arrow);

			//BitmapUtils.createBitmapFromAndroidBitmap(infoImage)
			//BitmapUtils.createBitmapFromAndroidBitmap(arrowImage);

			// Add popup
			BalloonPopupStyleBuilder builder = new BalloonPopupStyleBuilder();

			builder.LeftMargins = new BalloonPopupMargins(6, 6, 6, 6);
			builder.LeftImage = leftImage;

			builder.RightImage = rightImage;
			builder.RightMargins = new BalloonPopupMargins(2, 6, 12, 6);

			builder.CornerRadius = 20;
			builder.PlacementPriority = 1;

			MapPos position = projection.FromWgs84(new MapPos(24.655662, 59.425521));
			BalloonPopup popup1 = new BalloonPopup(position, builder.BuildStyle(), "Popup with pos", "Images, round");

			popup1.SetMetaDataElement("ClickText", new Variant("Popup nr 1"));
			source.Add(popup1);
		}

		public static void AddBalloonPopup2(LocalVectorDataSource source, Projection projection, 
		                                    Bitmap leftImage, Bitmap rightImage, Bitmap markerImage)
		{
			// Add popup, but instead of giving it a position attach it to a marker
			BalloonPopupStyleBuilder builder = new BalloonPopupStyleBuilder();
			builder.Color = new Color(0, 0, 0, 255);
			builder.CornerRadius = 0;

			builder.LeftMargins = new BalloonPopupMargins(6, 6, 6, 6);
			builder.LeftImage = leftImage;

			builder.RightImage = rightImage;
			builder.RightMargins = new BalloonPopupMargins(2, 6, 12, 6);

			builder.TitleColor = new Color(255, 255, 255, 255);
			builder.TitleFontName = "HelveticaNeue-Medium";
			builder.DescriptionColor = new Color(255, 255, 255, 255);
			builder.DescriptionFontName = "HelveticaNeue-Medium";

			builder.StrokeColor = new Color(255, 0, 120, 255);
			builder.StrokeWidth = 0;

			builder.PlacementPriority = 1;

			Marker marker = GetMarker(projection, new MapPos(24.646469, 59.426939), markerImage);
			source.Add(marker);

			Marker marker2 = GetMarker(projection, new MapPos(24.666469, 59.422939), markerImage);
			source.Add(marker2);

			BalloonPopup popup = new BalloonPopup(marker, builder.BuildStyle(), "Popup attached to marker", "Black, rectangle.");
			popup.SetMetaDataElement("ClickText", new Variant("Popup nr 2"));

			source.Add(popup);
		}

		public static void AddBalloonPopup3(LocalVectorDataSource source, Projection projection)
		{
			// Add popup
			BalloonPopupStyleBuilder builder = new BalloonPopupStyleBuilder();
			builder.DescriptionWrap = false;
			builder.PlacementPriority = 1;

			MapPos position = projection.FromWgs84(new MapPos(24.658662, 59.432521));
			String title = "This title will be wrapped if there's not enough space on the screen.";
			String description = "Description is set to be truncated with three dots, unless the screen is really really big.";

			BalloonPopup popup = new BalloonPopup(position, builder.BuildStyle(), title, description);

			popup.SetMetaDataElement("ClickText", new Variant("Popup nr 3"));

			source.Add(popup);
		}

		public static void Add2DPolygon(LocalVectorDataSource source, Projection projection)
		{
			LineStyleBuilder lineBuilder = new LineStyleBuilder();
			lineBuilder.Color = new Color(0, 0, 0, 255); // Black
			lineBuilder.Width = 1.0f;

			// Create polygon style and poses
			PolygonStyleBuilder polygonBuilder = new PolygonStyleBuilder();
			polygonBuilder.Color = new Color(255, 0, 0, 255); // Red
			polygonBuilder.LineStyle = lineBuilder.BuildStyle();

			MapPosVector polygonPoses = new MapPosVector();
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.650930, 59.421659)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.657453, 59.416354)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.661187, 59.414607)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.667667, 59.418123)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.665736, 59.421703)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.661444, 59.421245)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.660199, 59.420677)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.656552, 59.420175)));
			polygonPoses.Add(projection.FromWgs84(new MapPos(24.654010, 59.421472)));

			// Create 2 polygon holes
			MapPosVector holePoses1 = new MapPosVector();
			holePoses1.Add(projection.FromWgs84(new MapPos(24.658409, 59.420522)));
			holePoses1.Add(projection.FromWgs84(new MapPos(24.662207, 59.418896)));
			holePoses1.Add(projection.FromWgs84(new MapPos(24.662207, 59.417411)));
			holePoses1.Add(projection.FromWgs84(new MapPos(24.659524, 59.417171)));
			holePoses1.Add(projection.FromWgs84(new MapPos(24.657615, 59.419834)));

			MapPosVector holePoses2 = new MapPosVector();
			holePoses2.Add(projection.FromWgs84(new MapPos(24.665640, 59.421243)));
			holePoses2.Add(projection.FromWgs84(new MapPos(24.668923, 59.419463)));
			holePoses2.Add(projection.FromWgs84(new MapPos(24.662893, 59.419365)));

			MapPosVectorVector polygonHoles = new MapPosVectorVector();
			polygonHoles.Add(holePoses1);
			polygonHoles.Add(holePoses2);

			// Add polygon
			Polygon polygon = new Polygon(polygonPoses, polygonHoles, polygonBuilder.BuildStyle());
			polygon.SetMetaDataElement("ClickText", new Variant("Polygon"));
			source.Add(polygon);
		}

		public static void Add3DCar(LocalVectorDataSource source, Projection projection)
		{
			// Add a single 3D model to the vector layer
			String modelName = "milktruck.nml";

			MapPos modelPos = projection.FromWgs84(new MapPos(24.646469, 59.423939));
			NMLModel model = new NMLModel(modelPos, AssetUtils.LoadAsset(modelName));

			model.Scale = 20;
			model.SetMetaDataElement("ClickText", new Variant("Single model"));

			source.Add(model);
		}

		public static void Add3DPolygon(LocalVectorDataSource source, Projection projection)
		{
			// Create 3d polygon style and poses
			Polygon3DStyleBuilder polygon3DStyleBuilder = new Polygon3DStyleBuilder();
			polygon3DStyleBuilder.Color = new Color(51, 51, 255, 255);

			MapPosVector polygon3DPoses = new MapPosVector();
			polygon3DPoses.Add(projection.FromWgs84(new MapPos(24.635930, 59.416659)));
			polygon3DPoses.Add(projection.FromWgs84(new MapPos(24.642453, 59.411354)));
			polygon3DPoses.Add(projection.FromWgs84(new MapPos(24.646187, 59.409607)));
			polygon3DPoses.Add(projection.FromWgs84(new MapPos(24.652667, 59.413123)));
			polygon3DPoses.Add(projection.FromWgs84(new MapPos(24.650736, 59.416703)));
			polygon3DPoses.Add(projection.FromWgs84(new MapPos(24.646444, 59.416245)));

			// Create 3d polygon holes poses
			MapPosVector holePositions = new MapPosVector();
			holePositions.Add(projection.FromWgs84(new MapPos(24.643409, 59.411922)));
			holePositions.Add(projection.FromWgs84(new MapPos(24.651207, 59.412896)));
			holePositions.Add(projection.FromWgs84(new MapPos(24.643207, 59.414411)));

			MapPosVectorVector holes = new MapPosVectorVector();
			holes.Add(holePositions);

			// Add to datasource
			Polygon3D polygon = new Polygon3D(polygon3DPoses, holes, polygon3DStyleBuilder.BuildStyle(), 150);
			polygon.SetMetaDataElement("ClickText", new Variant("Polygon 3D"));
			source.Add(polygon);
		}

		static Marker GetMarker(Projection projection, MapPos position, Bitmap image)
		{
			//TODO
			//Bitmap androidMarkerBitmap = BitmapFactory.decodeResource(getResources(), R.drawable.marker);
			//com.carto.graphics.Bitmap markerBitmap = BitmapUtils.createBitmapFromAndroidBitmap(androidMarkerBitmap);

			// Create marker style
			MarkerStyleBuilder builder = new MarkerStyleBuilder();
			builder.Bitmap = image;
			builder.Size = 30;

			MarkerStyle style = builder.BuildStyle();

			// Create and return marker
			return new Marker(projection.FromWgs84(position), style);
		}

		static Point GetPoint(Projection projection, MapPos position, Color color)
		{
			PointStyleBuilder pointStyleBuilder = new PointStyleBuilder();
			pointStyleBuilder.Color = color;
			pointStyleBuilder.Size = 16;

			return new Point(projection.FromWgs84(position), pointStyleBuilder.BuildStyle());
		}
	}
}

