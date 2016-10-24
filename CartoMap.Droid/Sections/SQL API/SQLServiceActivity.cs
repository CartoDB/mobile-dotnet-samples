using System;
using Android.App;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Services;
using Carto.Styles;
using Shared;
using Shared.Droid;

namespace CartoMap.Droid
{
	[Activity]
	[ActivityDescription(Description = "Displays cities on the map via SQL query")]
	public class SQLServiceActivity : MapBaseActivity
	{
		const string query = "SELECT * FROM cities15000 WHERE population > 100000";

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Projection projection = MapView.Options.BaseProjection;

			// Create a datasource and layer for the map
			LocalVectorDataSource source = new LocalVectorDataSource(projection);
			VectorLayer layer = new VectorLayer(source);
			MapView.Layers.Add(layer);

			// Initialize CartoSQL service, set a username
			CartoSQLService service = new CartoSQLService();
			service.Username = "nutiteq";

			PointStyleBuilder builder = new PointStyleBuilder
			{
				Color = new Carto.Graphics.Color(255, 0, 0, 255),
				Size = 1
			};

			MapView.QueryFeatures(service, source, builder.BuildStyle(), query);
		}
	}
}

