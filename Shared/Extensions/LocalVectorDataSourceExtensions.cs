using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Styles;
using Carto.VectorElements;

namespace Shared
{
	public static class LocalVectorDataSourceExtensions
	{
		public static void AddPoint(this LocalVectorDataSource source, MapPos position)
		{
			PointStyleBuilder pointStyleBuilder = new PointStyleBuilder();
			pointStyleBuilder.Color = new Color(0, 0, 255, 255);

			Point point = new Point(position, pointStyleBuilder.BuildStyle());

			source.Add(point);
		}

		public static void AddLine(this LocalVectorDataSource source, MapPosVector positions)
		{
			LineStyleBuilder lineStyleBuilder = new LineStyleBuilder();
			lineStyleBuilder.Color = new Color(255, 0, 0, 255);

			Line line = new Line(positions, lineStyleBuilder.BuildStyle());

			source.Add(line);
		}

		public static void AddPolygon(this LocalVectorDataSource source, MapPosVector positions)
		{
			PolygonStyleBuilder polygonStyleBuilder = new PolygonStyleBuilder();
			polygonStyleBuilder.Color = new Color(0, 255, 0, 255);

			Polygon polygon = new Polygon(positions, polygonStyleBuilder.BuildStyle());

			source.Add(polygon);
		}
	}
}

