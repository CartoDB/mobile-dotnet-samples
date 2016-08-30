
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Graphics;
using Carto.Layers;
using Carto.Styles;
using Shared;

namespace CartoMap.iOS
{
	public class CartoSQLController : MapBaseController
	{
		public override string Name { get { return "Carto SQL Map"; } }

		public override string Description { get { return "SQL API to get data and create a custom vector data source"; } }

		const string BaseUrl = "https://nutiteq.cartodb.com/api/v2/sql";

		const string Query =
			"SELECT cartodb_id,the_geom_webmercator AS the_geom,name,address,bikes,slot," +
			"field_8,field_9,field_16,field_17,field_18 FROM stations_1 WHERE !bbox!";

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Define style for vector objects. 
			// Note that all objects must have same style here, which can be big limitation
			PointStyleBuilder builder = new PointStyleBuilder();
			builder.Color = new Color(0, 0, 255, 255);
			builder.Size = 10;

			PointStyle style = builder.BuildStyle();

			// Initialize a local vector data source
			CartoDBSQLDataSource dataSource = new CartoDBSQLDataSource(BaseProjection, BaseUrl, Query, style);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer1 = new VectorLayer(dataSource);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer1);

			// Set visible zoom range for the vector layer
			vectorLayer1.VisibleZoomRange = new MapRange(14, 23);

			// Initialize a local vector data source and layer for click Balloons
			LocalVectorDataSource vectorDataSource = new LocalVectorDataSource(BaseProjection);

			// Initialize a vector layer with the previous data source
			VectorLayer vectorLayer = new VectorLayer(vectorDataSource);

			// Add the previous vector layer to the map
			MapView.Layers.Add(vectorLayer);

			// Set listener to get point click popups
			MapView.MapEventListener = new MyMapEventListener(MapView, vectorDataSource);

			// Animate map to the marker
			MapPos newYork = BaseProjection.FromWgs84(new MapPos(-74.0059, 40.7127));
			MapView.SetFocusPos(newYork, 1);
			MapView.SetZoom(15, 1);
		}

	}
}