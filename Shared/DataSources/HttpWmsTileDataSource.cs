
using System;
using Carto.Core;
using Carto.DataSources;
using Carto.Projections;

namespace Shared
{
public class HttpWmsTileDataSource : HTTPTileDataSource
	{
		const int tileSize = 256;

		string baseUrl;

		string layer;
		string format;
		string style;
		bool wgsWms;
		Projection projection;

		/**
		 * _________________________________________________
		 * Creates WMS DataSource, based on tiles. 
		 * Map service must support EPSG:3857 or EPSG:4326. 
		 *     - EPSG:3857 has full compatibility and accuracy, 
		 *     - many services with EPSG:4326 work also, 
		 *       but on low zooms (world view) it is inaccurate,
		 *       because map display projection is always EPSG:3857 (in SDK 3.0)
		 *     
		 *     Only GetMap is implemented
		 * 
		 * * minZoom minimal zoom
		 * * maxZoom max zoom for map server
		 * * proj datasource projection, currently should be EPSG:3857
		 * * wgsWms false - uses EPSG:3857 for server, 
		 *   true - uses WGS84 (EPSG:4326) which is less accurate in low zooms
		 * * baseUrl You need to configure direct map URL, 
		 *   GetCapabilities is NOT used here
		 * * style - usually empty string, comma separated
		 * * layer comma-separated list of layers
		 * * format e.g. image/png, image/jpeg
		 * _________________________________________________
		 */
		public HttpWmsTileDataSource(int minZoom, int maxZoom, Projection proj, bool wgsWms,
			string baseUrl, string style, string layer, string format) : base(minZoom, maxZoom, baseUrl)
		{
			this.baseUrl = baseUrl;
			this.style = style;
			this.layer = layer;
			this.format = format;
			this.wgsWms = wgsWms;
			this.projection = proj;
		}

		protected override string BuildTileURL(string baseURL, MapTile tile)
		{
			string srs = "EPSG:3857";
			string bbox = GetTileBbox(tile);

			if (wgsWms)
			{
				srs = "EPSG:4326";
			}

			// Example Uri:
			// http://basemap.nationalmap.gov/arcgis/services/USGSTopo/MapServer/WmsServer?
			// LAYERS=0&FORMAT=image%2Fpng8&SERVICE=WMS&VERSION=1.1.0&REQUEST=GetMap&STYLES=
			// &EXCEPTIONS=application%2Fvnd.ogc.se_inimage&SRS=EPSG%3A3857&WIDTH=256&HEIGHT=256
			// &BBOX=-20037508.3427892%2C0%2C0%2C20037508.3427892

			string url = baseUrl;

			// Extension method; cf. iOS Extensions class
			url = url.Append("LAYERS", layer);
			url = url.Append("FORMAT", format);
			url = url.Append("SERVICE", "WMS");
			url = url.Append("VERSION", "1.1.0");
			url = url.Append("REQUEST", "GetMap");
			url = url.Append("STYLES", style);
			url = url.Append("EXCEPTIONS", "application/vnd.ogc.se_inimage");
			url = url.Append("SRS", srs);
			url = url.Append("WIDTH", tileSize.ToString());
			url = url.Append("HEIGHT", tileSize.ToString());

			url = url.Append("BBOX", bbox);

			return url;
		}

		string GetTileBbox(MapTile tile)
		{
			MapBounds envelope = GetTileBounds(tile.X, tile.Y, tile.Zoom, projection);

			// Convert corners to WGS84 if maps server needs it.
			if (wgsWms)
			{
				envelope = new MapBounds(projection.ToWgs84(envelope.Min), projection.ToWgs84(envelope.Max));
			}

			return EncodeBBox(envelope);
		}

		public string EncodeBBox(MapBounds envelope)
		{
			// ToInvariantString(): Convenience method to ignore culture info
			return
				envelope.Min.X.ToInvariantString() + "," +
				envelope.Min.Y.ToInvariantString() + "," +
				envelope.Max.X.ToInvariantString() + "," +
				envelope.Max.Y.ToInvariantString();
		}

		public MapBounds GetTileBounds(int tx, int ty, int zoom, Projection proj)
		{
			MapBounds bounds = proj.Bounds;

			// World size (bounds), approx 40000km
			double boundWidth = bounds.Max.X - bounds.Min.X;
			double boundHeight = bounds.Max.Y - bounds.Min.Y;

				
			int xCount = Math.Max(1, (int)Math.Round(boundWidth / boundHeight));
			int yCount = Math.Max(1, (int)Math.Round(boundHeight / boundWidth));

			// Resolution
			double resx = boundWidth / xCount / (tileSize * (double)(1 << (zoom)));
			double resy = boundHeight / yCount / (tileSize * (double)(1 << (zoom)));

			double minx = ((double)tx + 0) * tileSize * resx + bounds.Min.X;
			double maxx = ((double)tx + 1) * tileSize * resx + bounds.Min.X;
			double miny = -((double)ty + 1) * tileSize * resy + bounds.Max.Y;
			double maxy = -((double)ty + 0) * tileSize * resy + bounds.Max.Y;

			MapBounds env = new MapBounds(new MapPos(minx, miny), new MapPos(maxx, maxy));

			return env;
		}

	}
}

