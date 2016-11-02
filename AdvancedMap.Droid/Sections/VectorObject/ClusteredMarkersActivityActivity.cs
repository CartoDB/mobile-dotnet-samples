using System;
using Java.IO;
using Android.App;
using Carto.Layers;
using Carto.Utils;
using Shared.Droid;
using Shared;
using Carto.DataSources;
using System.Collections.Generic;
using Carto.Styles;
using Android.Content;
using Carto.VectorElements;
using Carto.Core;
using Carto.Geometry;

namespace AdvancedMap.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	[ActivityData(Title = "Clustered Markers", Description = "Read data from .geojson and show as clusters")]
	public class ClusteredMarkersActivity: MapBaseActivity
	{
		protected override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			AddBaseLayer(CartoBaseMapStyle.CartoBasemapStyleGray);

			// read json from assets and add to map
			string json;

			using (System.IO.StreamReader sr = new System.IO.StreamReader (Assets.Open ("cities15000.geojson")))
			{
				json = sr.ReadToEnd ();
			}

			// Initialize a local vector data source
			LocalVectorDataSource source = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			ClusteredVectorLayer layer = new ClusteredVectorLayer(source, new MyClusterElementBuilder(this));
			layer.MinimumClusterDistance = 50;

			new System.Threading.Thread((obj) =>
			{

				// Create a basic style, as the ClusterElementBuilder will set the real style
				MarkerStyle style = new MarkerStyleBuilder().BuildStyle();

				// Read GeoJSON, parse it using SDK GeoJSON parser
				GeoJSONGeometryReader reader = new GeoJSONGeometryReader();

				// Set target projection to base (mercator)
				reader.TargetProjection = BaseProjection;
				Alert("Starting load from .geojson");

				// Read features from local asset
				FeatureCollection features = reader.ReadFeatureCollection(json);
				Alert("Finished load from .geojson");

				for (int i = 0; i < features.FeatureCount; i++)
				{
					// This data set features point geometry,
					// however, it can also be LineGeometry or PolygonGeometry
					PointGeometry geometry = (PointGeometry)features.GetFeature(i).Geometry;
					source.Add(new Marker(geometry, style));
				}

				Alert("Finished adding Markers to source. Clustering started");

				// Add the clustered vector layer to the map
				MapView.Layers.Add(layer);

			}).Start();
		}
	}

	public class MyClusterElementBuilder : ClusterElementBuilder
	{
		Dictionary<int, MarkerStyle> markerStyles = new Dictionary<int, MarkerStyle>();
		Android.Graphics.Bitmap markerBitmap;


		public MyClusterElementBuilder(Context context)
		{
			markerBitmap = Android.Graphics.BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.marker_black);
		}

		public override VectorElement BuildClusterElement(MapPos pos, VectorElementVector elements)
		{
			MarkerStyle style = null;

			// Try to reuse existing marker styles
			if (markerStyles.ContainsKey(elements.Count)) {
				style = markerStyles[elements.Count];
			}
			    
			if (elements.Count == 1)
			{
				style = (elements[0] as Marker).Style;
			}

			if (style == null)
			{
				Android.Graphics.Bitmap canvasBitmap = markerBitmap.Copy(Android.Graphics.Bitmap.Config.Argb8888, true);
				Android.Graphics.Canvas canvas = new Android.Graphics.Canvas(canvasBitmap);

				Android.Graphics.Paint paint = new Android.Graphics.Paint(Android.Graphics.PaintFlags.AntiAlias);

				paint.TextAlign = (Android.Graphics.Paint.Align.Center);
				paint.TextSize = 12;
				paint.Color = Android.Graphics.Color.Argb(255, 0, 0, 0);

				float x = markerBitmap.Width / 2;
				float y = markerBitmap.Height / 2 - 5;

				canvas.DrawText(elements.Count.ToString(), x, y, paint);

				MarkerStyleBuilder styleBuilder = new MarkerStyleBuilder();
				styleBuilder.Bitmap = BitmapUtils.CreateBitmapFromAndroidBitmap(canvasBitmap);
				styleBuilder.Size = 30;
				styleBuilder.PlacementPriority = -elements.Count;

				style = styleBuilder.BuildStyle();

				markerStyles.Add(elements.Count, style);
			}

			// Create marker for the cluster
			Marker marker = new Marker(pos, style);
			return marker;
		}
	}
}

