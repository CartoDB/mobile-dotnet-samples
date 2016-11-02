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
			PointStyleBuilder builder = new PointStyleBuilder();
			builder.Size = 30;
			builder.Color = new Color(0, 0, 255, 255);

			Point point = new Point(position, builder.BuildStyle());

			source.Add(point);
		}

		public static void AddLine(this LocalVectorDataSource source, MapPosVector positions)
		{
			LineStyleBuilder builder = new LineStyleBuilder();
			builder.Width = 20;
			builder.ClickWidth = 40;
			builder.Color = new Color(255, 0, 0, 255);

			Line line = new Line(positions, builder.BuildStyle());

			source.Add(line);
		}

		public static void AddPolygon(this LocalVectorDataSource source, MapPosVector positions)
		{
			PolygonStyleBuilder builder = new PolygonStyleBuilder();
			builder.Color = new Color(0, 255, 0, 255);

			Polygon polygon = new Polygon(positions, builder.BuildStyle());

			source.Add(polygon);
		}
	}
}

