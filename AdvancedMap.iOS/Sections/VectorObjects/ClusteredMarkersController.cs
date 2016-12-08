
using System;
using System.Collections.Generic;
using Carto.Core;
using Carto.DataSources;
using Carto.Geometry;
using Carto.Layers;
using Carto.Styles;
using Carto.Utils;
using Carto.VectorElements;
using CoreGraphics;
using Foundation;
using Shared;
using Shared.iOS;
using UIKit;

namespace AdvancedMap.iOS
{
	public class ClusteredMarkersController : MapBaseController
	{
		public override string Name { get { return "Clustered Markers"; } }

		public override string Description { get { return "Show 20000 clustered points from geojson"; } }

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			// Add default base layer
			AddOnlineBaseLayer(CartoBaseMapStyle.CartoBasemapStyleGray);

			// Initialize a local vector data source
			LocalVectorDataSource source = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			var layer = new ClusteredVectorLayer(source, new MyClusterElementBuilder());

			// Default is 100. A good value depends on data
			layer.MinimumClusterDistance = 50;

			// read json from assets and add to map
			string json = System.IO.File.ReadAllText(AssetUtils.CalculateResourcePath("cities15000.geojson"));

			// Create a basic style, as the ClusterElementBuilder will set the real style
			MarkerStyle style = new MarkerStyleBuilder().BuildStyle();

			// Read GeoJSON, parse it using SDK GeoJSON parser
			GeoJSONGeometryReader reader = new GeoJSONGeometryReader();

			// Set target projection to base (mercator)
			reader.TargetProjection = BaseProjection;

			// Read features from local asset
			FeatureCollection features = reader.ReadFeatureCollection(json);

			VectorElementVector elements = new VectorElementVector();

			for (int i = 0; i < features.FeatureCount; i++)
			{
				// This data set features point geometry,
				// however, it can also be LineGeometry or PolygonGeometry
				PointGeometry geometry = (PointGeometry)features.GetFeature(i).Geometry;
				elements.Add(new Marker(geometry, style));
			}

			// Add the clustered vector layer to the map
			source.AddAll(elements);

			MapView.Layers.Add(layer);
		}
	}

	public class MyClusterElementBuilder : ClusterElementBuilder
	{
		Dictionary<int, MarkerStyle> markerStyles = new Dictionary<int, MarkerStyle>();
		UIImage markerImage;

		public MyClusterElementBuilder()
		{
			markerImage = UIImage.FromFile("icons/marker_black.png");
		}

		public override VectorElement BuildClusterElement(MapPos pos, VectorElementVector elements)
		{
			// Try to reuse existing marker styles
			MarkerStyle style = null;

			style = FindByKey(elements.Count);

			if (elements.Count == 1)
			{
				style = ((Marker)elements[0]).Style;
			}

			if (style == null)
			{
				UIFont font = UIFont.FromName("Helvetica Neue", 10);
				UIGraphics.BeginImageContext(markerImage.Size);

				// Draw existing image on new image context;
				markerImage.Draw(new CGPoint(0, 0));

				NSString native = new NSString(elements.Count.ToString());

				// Find the center location and draw text there
				nfloat y = markerImage.Size.Height / 4;
				CGRect rectangle = new CGRect(0, y, markerImage.Size.Width, markerImage.Size.Height);
				native.DrawString(rectangle, font, UILineBreakMode.WordWrap, UITextAlignment.Center);

				// Extract image
				UIImage newImage = UIGraphics.GetImageFromCurrentImageContext();

				UIGraphics.EndImageContext();

				MarkerStyleBuilder styleBuilder = new MarkerStyleBuilder();
				styleBuilder.Bitmap = BitmapUtils.CreateBitmapFromUIImage(newImage);
				styleBuilder.Size = 30;
				styleBuilder.PlacementPriority = -elements.Count;

				style = styleBuilder.BuildStyle();

				markerStyles.Add(elements.Count, style);
			}

			// Create marker for the cluster
			Marker marker = new Marker(pos, style);
			return marker;
		}

		MarkerStyle FindByKey(int key)
		{
			foreach (KeyValuePair<int, MarkerStyle> style in markerStyles) {
				if (key == style.Key) {
					return style.Value;
				}
			}

			return null;
		}
	}
}

