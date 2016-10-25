using System;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace Shared
{
	public class BasicEditEventListener : VectorEditEventListener
	{
		PointStyle styleNormal, styleVirtual, styleSelected;

		LocalVectorDataSource source;

		public BasicEditEventListener(LocalVectorDataSource source)
		{
			this.source = source;
		}

		public override void OnElementModify(VectorElement element, Geometry geometry)
		{
			if (element is Point && geometry is PointGeometry)
			{
				(element as Point).Geometry = (PointGeometry)geometry;
			}
			else if (element is Line && geometry is LineGeometry)
			{
				(element as Line).Geometry = (LineGeometry)geometry;
			}
			else if (element is Polygon && geometry is PolygonGeometry)
			{
				(element as Polygon).Geometry = (PolygonGeometry)geometry;
			}
		}

		public override void OnElementDelete(VectorElement element)
		{
			source.Remove(element);
		}

		public override VectorElementDragResult OnDragStart(VectorElementDragInfo dragInfo)
		{
			return VectorElementDragResult.VectorElementDragResultModify;
		}

		public override VectorElementDragResult OnDragMove(VectorElementDragInfo dragInfo)
		{
			return VectorElementDragResult.VectorElementDragResultModify;
		}

		public override VectorElementDragResult OnDragEnd(VectorElementDragInfo dragInfo)
		{
			return VectorElementDragResult.VectorElementDragResultModify;
		}

		public override PointStyle OnSelectDragPointStyle(VectorElement element, VectorElementDragPointStyle dragPointStyle)
		{
			if (styleNormal == null)
			{
				PointStyleBuilder builder = new PointStyleBuilder();
				builder.Color = new Color(0, 255, 255, 255);
				builder.Size = 20;

				styleNormal = builder.BuildStyle();

				builder.Size = 15;

				styleVirtual = builder.BuildStyle();

				builder.Color = new Color(255, 255, 0, 255);
				builder.Size = 30;

				styleSelected = builder.BuildStyle();
			}

			if (dragPointStyle == VectorElementDragPointStyle.VectorElementDragPointStyleSelected)
			{
				return styleSelected;
			}

			if (dragPointStyle == VectorElementDragPointStyle.VectorElementDragPointStyleVirtual)
			{
				return styleVirtual;
			}

			return styleNormal;

		}
	}
}

