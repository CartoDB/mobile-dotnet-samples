
using System;
using Android.App;
using Android.Graphics;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorElements;

namespace CartoMobileSample
{
	[Activity]
	[ActivityDescription(Description = 
	                     "A sample demonstrating how to use markers on the map: " +
	                     "creating a data source, creating a layer, loading marker bitmaps, creating a style " +
	                     "and adding the marker to the data source.")]
	public class PinMapActivity : VectorBaseMapActivity
	{
		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Add a pin marker to map
			// 1. Initialize a local vector data source
			LocalVectorDataSource vectorDataSource1 = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer1 = new VectorLayer(vectorDataSource1);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer1);

			// Set visible zoom range for the vector layer
			vectorLayer1.VisibleZoomRange = new MapRange(0, 18);

			// Create marker style
			Bitmap androidMarkerBitmap = BitmapFactory.DecodeResource(Resources, HelloMap.Resource.Drawable.marker);
			Carto.Graphics.Bitmap markerBitmap = BitmapUtils.CreateBitmapFromAndroidBitmap(androidMarkerBitmap);

			MarkerStyleBuilder markerStyleBuilder = new MarkerStyleBuilder();
			markerStyleBuilder.Bitmap = markerBitmap;

			markerStyleBuilder.Size = 30;
			MarkerStyle sharedMarkerStyle = markerStyleBuilder.BuildStyle();

			// 3. Add marker
			//MapPos berlin = new MapPos(13.38933, 52.51704);
			MapPos tallinn = new MapPos(24.646469, 59.426939);

			MapPos markerPos = MapView.Options.BaseProjection.FromWgs84(tallinn);
			Marker marker1 = new Marker(markerPos, sharedMarkerStyle);
			vectorDataSource1.Add(marker1);

			// Animate map to the marker
			MapView.SetFocusPos(markerPos, 1);
			MapView.SetZoom(12, 1);
		}
	}
}

