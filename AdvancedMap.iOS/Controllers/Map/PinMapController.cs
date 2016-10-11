
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorElements;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class PinMapController : VectorMapBaseController
	{
		public override string Name { get { return "Pin Map"; } }

		public override string Description
		{
			get
			{
				return "Creating data sources, layers, styles & loading marker bitmaps and adding them to the data source";
			}
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			//MapPos berlin = new MapPos(13.38933, 52.51704);
			MapPos tallinn = new MapPos(24.646469, 59.426939);

			AddPinTo(tallinn);
		}

		void AddPinTo(MapPos position)
		{
			// 1. Initialize a local vector data source
			LocalVectorDataSource vectorDataSource1 = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer1 = new VectorLayer(vectorDataSource1);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer1);

			// Set visible zoom range for the vector layer
			vectorLayer1.VisibleZoomRange = new MapRange(0, 18);

			// Create marker style
			Carto.Graphics.Bitmap markerBitmap = BitmapUtils.CreateBitmapFromUIImage(UIImage.FromFile("marker.png"));
			MarkerStyleBuilder markerStyleBuilder = new MarkerStyleBuilder();
			markerStyleBuilder.Bitmap = markerBitmap;

			markerStyleBuilder.Size = 30;
			MarkerStyle sharedMarkerStyle = markerStyleBuilder.BuildStyle();

			// 3. Add marker
			MapPos markerPos = MapView.Options.BaseProjection.FromWgs84(position);
			Marker marker1 = new Marker(markerPos, sharedMarkerStyle);
			vectorDataSource1.Add(marker1);

			// Animate map to the marker
			MapView.SetFocusPos(markerPos, 1);
			MapView.SetZoom(12, 1);
		}
	}
}

