
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.DataSources.Components;
using Carto.Geometry;
using Carto.Projections;
using Carto.Renderers.Components;
using Carto.Styles;
using Carto.VectorElements;
using Java.IO;
using Java.Lang;
using Java.Net;
using Org.Json;

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
				encodedQuery = URLEncoder.Encode(unencodedQuery, "UTF-8");
			}
			catch (UnsupportedEncodingException e)
			{
				e.PrintStackTrace();
			}

			string urlAddress = baseUrl + "?format=GeoJSON&q=" + encodedQuery;

			try
			{
				URL url = new URL(urlAddress);

				BufferedReader streamReader = new BufferedReader(new InputStreamReader(url.OpenConnection().InputStream, "UTF-8"));
				StringBuilder responseStrBuilder = new StringBuilder();

				string inputStr;

				while ((inputStr = streamReader.ReadLine()) != null)
				{
					responseStrBuilder.Append(inputStr);
				}

				JSONObject json = new JSONObject(responseStrBuilder.ToString());

				GeoJSONGeometryReader geoJsonParser = new GeoJSONGeometryReader();

				JSONArray features = json.GetJSONArray("features");

				for (int i = 0; i < features.Length(); i++)
				{
					JSONObject feature = (JSONObject)features.Get(i);
					JSONObject geometry = feature.GetJSONObject("geometry");

					// Use SDK GeoJSON parser
					Geometry ntGeom = geoJsonParser.ReadGeometry(geometry.ToString());

					JSONObject properties = feature.GetJSONObject("properties");
					VectorElement element;

					// create object based on given style
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

					var iterator = properties.Keys();

					// Add all properties as MetaData, so you can use it with click handling
					while (iterator.HasNext) {
						string key = (string)iterator.Next();
						string val = properties.GetString(key);
						element.SetMetaDataElement(key, new Variant(val));
						System.Console.WriteLine("KEY: " + key + "; VAL: " + val);
					}

					elements.Add(element);
				}

			}

			catch (JSONException e)
			{
				e.PrintStackTrace();
			}
			catch (MalformedURLException e)
			{
				e.PrintStackTrace();
			}
			catch (UnsupportedEncodingException e)
			{
				e.PrintStackTrace();
			}
			catch (IOException e)
			{
				e.PrintStackTrace();
			}


		}
	}
}

