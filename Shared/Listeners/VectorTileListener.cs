
using System;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Carto.VectorElements;
using Carto.Ui;

namespace Shared
{
	public class VectorTileListener : VectorTileEventListener
	{
		public bool IsForce { get; set; } = false;

		public double Force { get; set; }

		VectorLayer layer;

		public VectorTileListener(VectorLayer layer)
		{
			this.layer = layer;
		}

		public override bool OnVectorTileClicked(VectorTileClickInfo clickInfo)
		{
			LocalVectorDataSource source = (LocalVectorDataSource)layer.DataSource;

			source.Clear();

			Color color = new Color(0, 100, 200, 150);

			Feature feature = clickInfo.Feature;
			Geometry geometry = feature.Geometry;

			PointStyleBuilder pointBuilder = new PointStyleBuilder();
			pointBuilder.Color = color;

			LineStyleBuilder lineBuilder = new LineStyleBuilder();
			lineBuilder.Color = color;

			PolygonStyleBuilder polygonBuilder = new PolygonStyleBuilder();
			polygonBuilder.Color = color;

			if (geometry is PointGeometry)
			{
				source.Add(new Point((PointGeometry)geometry, pointBuilder.BuildStyle()));
			}
			else if (geometry is LineGeometry)
			{
				source.Add(new Line((LineGeometry)geometry, lineBuilder.BuildStyle()));
			}
			else if (geometry is PolygonGeometry)
			{
				source.Add(new Polygon((PolygonGeometry)geometry, polygonBuilder.BuildStyle()));
			}
			else if (geometry is MultiGeometry)
			{
				GeometryCollectionStyleBuilder collectionBuilder = new GeometryCollectionStyleBuilder();
				collectionBuilder.PointStyle = pointBuilder.BuildStyle();
				collectionBuilder.LineStyle = lineBuilder.BuildStyle();
				collectionBuilder.PolygonStyle = polygonBuilder.BuildStyle();

				source.Add(new GeometryCollection((MultiGeometry)geometry, collectionBuilder.BuildStyle()));
			}

			BalloonPopupStyleBuilder builder = new BalloonPopupStyleBuilder();

			// Set a higher placement priority so it would always be visible
			builder.PlacementPriority = 10;

			string message;
			string name = feature.Properties.GetObjectElement("name").String;
			string description = feature.Properties.GetObjectElement("description").String.ToMax200Characters();

			if (name.Equals("null"))
			{
				string facility = feature.Properties.GetObjectElement("facility").String;

				if (!facility.Equals("null"))
				{
					message = facility;
				}
				else
				{
					message = "Building";
				}
			}
			else
			{
				message = name;

				if (!description.Equals("null"))
				{
					message += "\n" + description;
				}
			}

			BalloonPopup popup = new BalloonPopup(clickInfo.ClickPos, builder.BuildStyle(), "", message);

			source.Add(popup);

			if (IsForce)
			{
				Console.WriteLine("Force touch");
				return false;
			}

			Console.WriteLine("Non-force");
			return true;
		}
	}
}

