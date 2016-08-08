
using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;

using Carto.Core;
using Carto.DataSources;
using Carto.DataSources.Components;
using Carto.Geometry;
using Carto.Projections;
using Carto.Renderers.Components;
using Carto.Styles;
using Carto.VectorElements;

namespace CartoMobileSample
{
	/**
	* A custom vector data source making queries to http://docs.cartodb.com/cartodb-platform/sql-api/
	*/
	public class CartoDBSQLDataSource : VectorDataSource
	{
		string baseUrl;
		string query;

		Style style;

		public CartoDBSQLDataSource(Projection proj, string baseUrl, string query, Style style) : base(proj)
		{
			this.baseUrl = baseUrl;
			this.query = query;

			this.style = style;
		}

		public override VectorData LoadElements(CullState cullState)
		{
			VectorElementVector elements = new VectorElementVector();

			MapEnvelope mapViewBounds = cullState.GetProjectionEnvelope(this.Projection);
			MapPos min = mapViewBounds.Bounds.Min;
			MapPos max = mapViewBounds.Bounds.Max;

			// Run query here
			LoadData(elements, min, max, cullState.ViewState.Zoom);

			return new VectorData(elements);
		}

		void LoadData(VectorElementVector elements, MapPos min, MapPos max, float zoom)
		{
			// Load and parse JSON
			string format = "ST_SetSRID(ST_MakeEnvelope(%f,%f,%f,%f),3857) && the_geom_webmercator";
			string bbox = string.Format(format, min.X, min.Y, max.X, max.Y);

			string unencodedQuery = query.Replace("!bbox!", bbox);

			unencodedQuery = unencodedQuery.Replace("zoom('!scale_denominator!')", Convert.ToString(zoom));

			string encodedQuery = null;

			try
			{
				encodedQuery = System.Web.HttpUtility.UrlEncode(unencodedQuery, System.Text.Encoding.UTF8);
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}

			string urlAddress = baseUrl + "?format=GeoJSON&q=" + encodedQuery;

			try
			{
				string result = "";

				using (StreamReader reader = new StreamReader(urlAddress)) {
					result = reader.ReadToEnd();
				}

				JsonValue json = JsonValue.Parse(result);

				GeoJSONGeometryReader geoJsonParser = new GeoJSONGeometryReader();

				JsonArray features = (JsonArray)json["features"];

				for (int i = 0; i < features.Count; i++)
				{
					JsonObject feature = (JsonObject)features[i];
					JsonObject geometry = (JsonObject)feature["geometry"];

					// Use SDK GeoJSON parser
					Geometry ntGeom = geoJsonParser.ReadGeometry(geometry.ToString());

					JsonObject properties = (JsonObject)feature["properties"];
					VectorElement element;

					// Create object based on given style
					if (style is PointStyle)
					{
						element = new Point((PointGeometry)ntGeom, (PointStyle)style);
					}
					else if (style is MarkerStyle)
					{
						element = new Marker(ntGeom, (MarkerStyle)style);
					}
					else if (style is LineStyle)
					{
						element = new Line((LineGeometry)ntGeom, (LineStyle)style);
					}
					else if (style is PolygonStyle)
					{
						element = new Polygon((PolygonGeometry)ntGeom, (PolygonStyle)style);
					}
					else {
						System.Console.WriteLine("Object creation not implemented yet for style: " + style.SwigGetClassNameStyle());
						break;
					}

					// Add all properties as MetaData, so you can use it with click handling
					foreach (KeyValuePair<string, JsonValue> item in properties) 
					{
						//	string key = (string)iterator.Next();
						//	string val = properties.GetString(key);
						//	element.SetMetaDataElement(key, new Variant(val));
						//	System.Console.WriteLine("KEY: " + key + "; VAL: " + val);	
					}

					elements.Add(element);
				}
			}

			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
		}

	}
}

