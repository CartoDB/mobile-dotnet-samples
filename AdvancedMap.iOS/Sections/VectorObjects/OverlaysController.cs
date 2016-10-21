
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.Projections;
using Carto.Utils;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class OverlaysController : MapBaseController
	{
		public override string Name { get { return "Overlays"; } }

		public override string Description
		{
			get
			{
				return "2D and 3D objects: lines, points, polygons, texts, pop-ups and a NMLModel";
			}
		}

		Projection projection;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.projection = BaseProjection;

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

			Bitmap info = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("info.png"));
			Bitmap arrow = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("arrow.png"));
			Bitmap marker = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("marker.png"));

			Overlays.AddBalloonPopup1(source, projection, info, arrow);
			Overlays.AddBalloonPopup2(source, projection, info, arrow, marker);
			Overlays.AddBalloonPopup3(source, projection);

			Overlays.Add3DCar(source, projection);
			Overlays.Add3DPolygon(source, projection);

			// Animate map to Tallinn where the objects are
			MapView.SetFocusPos(projection.FromWgs84(new MapPos(24.662893, 59.419365)), 1);
			MapView.SetZoom(12, 1);

			// Add maplistener to detect click on model
			MapView.MapEventListener = new MyMapEventListener(MapView, source);
		}
	}
}

