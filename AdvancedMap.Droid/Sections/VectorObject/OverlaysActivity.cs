using System;
using Java.IO;
using Carto.Utils;
using Android.App;
using Carto.Layers;
using Shared.Droid;
using Shared;
using Carto.Projections;
using Carto.DataSources;
using Carto.Core;
using Carto.Graphics;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityData(Title = "Overlays", Description = "2D and 3D objects: lines, points, polygons, texts, pop-ups and a NMLModel")]
	public class OverlaysActivity: MapBaseActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Projection projection = BaseProjection;

			// Initialize an local vector data source
			LocalVectorDataSource source = new LocalVectorDataSource(projection);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer = new VectorLayer(source);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer);

			// Set visible zoom range for the vector layer
			vectorLayer.VisibleZoomRange = new MapRange(10, 24);

			// Add a bunch of vector elements
			// As elements can be shared across platforms, they are in a shared project
			Overlays.AddPoint1(source, projection);
			Overlays.AddPoint2(source, projection);

			Overlays.AddOverlyingLines(MapView, source, projection);

			Overlays.Add2DPolygon(source, projection);

			Overlays.AddText1(source, projection);
			Overlays.AddText2(source, projection);
			Overlays.AddText3(source, projection);

			Bitmap info = CreateBitmap(Resource.Drawable.info);
			Bitmap arrow = CreateBitmap(Resource.Drawable.arrow);
			Bitmap marker = CreateBitmap(Resource.Drawable.marker);

			Overlays.AddBalloonPopup1(source, projection, info, arrow);
			Overlays.AddBalloonPopup2(source, projection, info, arrow, marker);
			Overlays.AddBalloonPopup3(source, projection);

			Overlays.Add3DCar(source, projection);
			Overlays.Add3DPolygon(source, projection);

			// Animate map to Tallinn where the objects are
			MapView.SetFocusPos(projection.FromWgs84(new MapPos(24.662893, 59.419365)), 1);
			MapView.SetZoom(12, 1);

			// Add maplistener to detect click on model

			VectorElementListener listener = new VectorElementListener(source);
			for (int i = 0; i < MapView.Layers.Count; i++) 
			{
				Layer layer = MapView.Layers[i];

				if (layer is VectorLayer) 
				{
					(layer as VectorLayer).VectorElementEventListener = listener;
				}
			}
		}
	}
}

