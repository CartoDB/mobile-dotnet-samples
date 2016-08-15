using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
	// TODO: reimplement to use Carto.Services.CartoSQLService, current version contains code not working on UWP/Windows Phone 10
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
			string bbox = EncodeBBox(min.X, min.Y, max.X, max.Y);

			string unencoded = query.Replace("!bbox!", bbox);

			unencoded = unencoded.Replace("zoom('!scale_denominator!')", Convert.ToString(zoom));

			// UrlEncode does not replace parentheses by default as they are valid url elements
			string encoded = System.Web.HttpUtility.UrlEncode(unencoded).Replace("(", "%28").Replace(")", "%29");

			string fullPath = baseUrl + "?format=GeoJSON&q=" + encoded;

			try
			{
				string json = GetString(fullPath);

				GeoJSONGeometryReader geoJsonParser = new GeoJSONGeometryReader();
				FeatureCollection features = geoJsonParser.ReadFeatureCollection(json);

				for (int i = 0; i < features.FeatureCount; i++)
				{
					Geometry geom = features.GetFeature(i).Geometry;
					Variant props = features.GetFeature(i).Properties;

					// Create object based on given style
					VectorElement element;

					if (style is PointStyle)
					{
						element = new Point((PointGeometry)geom, (PointStyle)style);
					}
					else if (style is MarkerStyle)
					{
						element = new Marker(geom, (MarkerStyle)style);
					}
					else if (style is LineStyle)
					{
						element = new Line((LineGeometry)geom, (LineStyle)style);
					}
					else if (style is PolygonStyle)
					{
						element = new Polygon((PolygonGeometry)geom, (PolygonStyle)style);
					}
					else
					{
						System.Console.WriteLine("Object creation not implemented yet for style: " + style.SwigGetClassNameStyle());
						break;
					}

					// Add all properties as MetaData, so you can use it with click handling
					for (int j = 0; j < props.ObjectKeys.Count; j++)
					{
						var key = props.ObjectKeys[j];
						var val = props.GetObjectElement(key);
						element.SetMetaDataElement(key, val);
					}

					elements.Add(element);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception: " + e.Message);
			}
		}

		string EncodeBBox(double minx, double miny, double maxx, double maxy)
		{
			int roundBy = 6;

			string minX = Math.Round(minx, roundBy).ToInvariantString();
			string minY = Math.Round(miny, roundBy).ToInvariantString();
			string maxX = Math.Round(maxx, roundBy).ToInvariantString();
			string maxY = Math.Round(maxy, roundBy).ToInvariantString();

			return "ST_SetSRID(ST_MakeEnvelope(" + minX + "," + minY + "," + maxX + "," + maxY + "),3857) && the_geom_webmercator";
		}

		string GetString(string url)
		{
			using (StreamReader reader = new StreamReader(GetStreamFromUrl(url)))
			{
				return reader.ReadToEnd();
			}
		}

		Stream GetStreamFromUrl(string url)
		{
			byte[] data = null;

			using (var client = new WebClient())
			{
				data = client.DownloadData(url);
			}

			return new MemoryStream(data);
		}

	}
}

