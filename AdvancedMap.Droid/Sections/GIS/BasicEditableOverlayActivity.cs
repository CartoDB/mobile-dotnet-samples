
using System;
using Android.App;
using Android.Util;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Graphics;
using Carto.Layers;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity(Label = "")]
	[ActivityDescription(Description = "Shows usage of an editable vector layer")]
	public class BasicEditableOverlayActivity : MapBaseActivity
	{
		LocalVectorDataSource source;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Initialize source and Edit layer, add it to the map
			source = new LocalVectorDataSource(MapView.Options.BaseProjection);

			EditableVectorLayer editLayer = new EditableVectorLayer(source);
			MapView.Layers.Add(editLayer);

			// Convenience methods to add elements to the map, cf. LocalVectorDataSourceExtensions
			source.AddPoint(new MapPos(-5000000, -900000));

			source.AddLine(new MapPosVector { 
				new MapPos(-6000000, -500000), new MapPos(-9000000, -500000) 
			});

			source.AddPolygon(new MapPosVector { 
				new MapPos(-5000000, -5000000), new MapPos(5000000, -5000000), new MapPos(0, 10000000) 
			});

			// Add a vector element even listener to select elements (on element click)
			editLayer.VectorElementEventListener = new VectirElementSelectEventListener(editLayer);

			// Add a map even listener to deselect element (on map click)
			MapView.MapEventListener = new VectorElementDeselectEventListener(editLayer);

			// Add the vector element edit even listener
			editLayer.VectorEditEventListener = new BasicEditEventListener(source);
		}
	}

	public class VectorElementDeselectEventListener : MapEventListener
	{
		EditableVectorLayer vectorLayer;

		public VectorElementDeselectEventListener(EditableVectorLayer vectorLayer)
		{
			this.vectorLayer = vectorLayer;
		}

		public override void OnMapClicked(MapClickInfo mapClickInfo)
		{
			vectorLayer.SelectedVectorElement = null;
		}
	}

	public class VectirElementSelectEventListener : VectorElementEventListener
	{
		EditableVectorLayer vectorLayer;

		public VectirElementSelectEventListener(EditableVectorLayer vectorLayer)
		{
			this.vectorLayer = vectorLayer;
		}

		public override bool OnVectorElementClicked(VectorElementClickInfo clickInfo)
		{
			vectorLayer.SelectedVectorElement = clickInfo.VectorElement;
			return true;
		}
	}

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

