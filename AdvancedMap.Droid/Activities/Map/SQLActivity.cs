using System;
using Android.App;
using Android.OS;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Shared;
using Shared.Droid;

namespace AdvancedMap.Droid
{
	[Activity]
	[ActivityDescription(Description = "SQL API to get data and create a custom vector data source")]
	public class SQLActivity : VectorBaseMapActivity
	{
		const string BaseUrl = "https://nutiteq.cartodb.com/api/v2/sql";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			// MapSampleBaseActivity creates and configures mapView  
			base.OnCreate(savedInstanceState);

			// Define style for vector objects. Note that all objects must have same style here, which can be big limitation
			PointStyleBuilder pointStyleBuilder = new PointStyleBuilder();
			pointStyleBuilder.Color = new Color(0, 0, 255, 255);
			pointStyleBuilder.Size = 10;

			// Initialize a local vector data source
			string query = 
				"SELECT cartodb_id,the_geom_webmercator AS the_geom,name,address,bikes,slot," +
				"field_8,field_9,field_16,field_17,field_18 FROM stations_1 WHERE !bbox!";

			CartoDBSQLDataSource vectorDataSource1 = new CartoDBSQLDataSource(
				BaseProjection, 
				BaseUrl, 
				query, 
				pointStyleBuilder.BuildStyle()
			);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer1 = new VectorLayer(vectorDataSource1);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer1);

			// Set visible zoom range for the vector layer
			vectorLayer1.VisibleZoomRange = new MapRange(14, 23);


			// Set listener to get point click popups

			// Initialize a local vector data source and layer for click Balloons
			LocalVectorDataSource vectorDataSource = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer = new VectorLayer(vectorDataSource);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer);

			MapView.MapEventListener = new MyMapEventListener(MapView, vectorDataSource);

			// Animate map to the marker
			MapView.SetFocusPos(BaseProjection.FromWgs84(new MapPos(-74.0059, 40.7127)), 1);
			MapView.SetZoom(15, 1);
		}

		protected override void OnDestroy()
		{
			MapView.MapEventListener = null;

			base.OnDestroy();
		}
	}
}
