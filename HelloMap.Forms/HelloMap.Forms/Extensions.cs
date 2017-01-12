
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Layers;
using Carto.Projections;
using Carto.Styles;
using Carto.Ui;
using Carto.VectorElements;

namespace HelloMap.Forms
{
	public static class Extensions
	{
		public static Marker AddMarkerToPosition(this MapView map, MapPos position)
		{
			// Initialize a local vector data source
			Projection projection = map.Options.BaseProjection;
			LocalVectorDataSource datasource = new LocalVectorDataSource(projection);

			// Initialize a vector layer with the previous data source
			VectorLayer layer = new VectorLayer(datasource);

			// Add layer to map
			map.Layers.Add(layer);

			// Set marker style
			MarkerStyleBuilder builder = new MarkerStyleBuilder();
			builder.Size = 15;
			builder.Color = new Carto.Graphics.Color(0, 255, 0, 255);

			MarkerStyle style = builder.BuildStyle();

			// Create marker and add it to the source
			Marker marker = new Marker(position, style);
			datasource.Add(marker);

			return marker;
		}
	}
}
